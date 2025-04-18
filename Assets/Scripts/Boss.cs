using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject[] waypoints;
    int curr = 0;
    int next = 1;
    float ahead = 2.0f;
    float moveSpeed = 3.0f;
    float bulletspeed = 5.0f;
    float turnSpeed = 250.0f * Mathf.Deg2Rad;
    float detectRadius = 5.0f;

    public GameObject NormalBullet;
    public GameObject PoisonBullet;

    public float Boss_Health;
    float BoosStartHealth = 30;
    bool phase1 = true;

    float Cooldown = 2.0f;
    float Heatup;
    float Doomsday = 0.0f;

    Rigidbody2D rb;
    enum State
    {
        PATROL,
        ATTACK
    }

    State state = State.PATROL;
    // float AttackTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // AttackTimer += Time.deltaTime;
        Heatup = Time.deltaTime;
        if (Boss_Health <= BoosStartHealth/2)
        {
            phase1 = false;
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        if (Boss_Health <= 0)
        {
            Destroy(gameObject);
        }
        UpdateState();
        switch (state)
        {
            case State.PATROL:
                Patrol();
                break;

            case State.ATTACK:
                Attack();
                break;
        }
        rb.AddForce(Avoid());
        transform.up = Vector3.RotateTowards(transform.up, rb.linearVelocity.normalized, turnSpeed * Time.deltaTime, 0.0f);
        Debug.DrawLine(transform.position, transform.position + transform.up * 5.0f, Color.green);

        Cooldown -= Heatup;

    }
    void UpdateState()
    {
        Vector3 toPlayer = (player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer, detectRadius);
        bool playerHit = hit && hit.collider.CompareTag("Player");
        state = playerHit ? State.ATTACK : State.PATROL;
        //Debug.DrawLine(transform.position, transform.position + toPlayer * detectRadius, playerHit ? Color.red : Color.green);
    }

    void Patrol()
    {
        rb.AddForce(Steering.FollowLine(gameObject, waypoints, ref curr, ref next, moveSpeed, ahead));
    }
    void Shot(Vector3 direction)
    {
        if (phase1)
        {

            Vector3 directionLeft = Quaternion.Euler(0.0f, 0.0f, 20.0f) * direction;
            Vector3 directionRight = Quaternion.Euler(0.0f, 0.0f, -20.0f) * direction;
           

            GameObject bullet = Instantiate(NormalBullet);
            bullet.transform.position = transform.position + direction * 1.0f;
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletspeed;
            bullet.GetComponent<SpriteRenderer>().color = Color.red;

            GameObject bullet2 = Instantiate(NormalBullet);
            bullet2.transform.position = transform.position + direction * 1.0f;
            bullet2.GetComponent<Rigidbody2D>().linearVelocity = directionLeft * bulletspeed;
            bullet2.GetComponent<SpriteRenderer>().color = Color.red;

            GameObject bullet3 = Instantiate(NormalBullet);
            bullet3.transform.position = transform.position + direction * 1.0f;
            bullet3.GetComponent<Rigidbody2D>().linearVelocity = directionRight * bulletspeed;
            bullet3.GetComponent<SpriteRenderer>().color = Color.red;

        

        }
        else
        {
            Vector3 directionLeft = Quaternion.Euler(0.0f, 0.0f, 20.0f) * direction;
            Vector3 directionRight = Quaternion.Euler(0.0f, 0.0f, -20.0f) * direction;


            GameObject bullet = Instantiate(PoisonBullet);
            bullet.transform.position = transform.position + direction * 1.0f;
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletspeed;
            bullet.GetComponent<SpriteRenderer>().color = Color.green;

            GameObject bullet2 = Instantiate(PoisonBullet);
            bullet2.transform.position = transform.position + direction * 1.0f;
            bullet2.GetComponent<Rigidbody2D>().linearVelocity = directionLeft * bulletspeed;
            bullet2.GetComponent<SpriteRenderer>().color = Color.green;

            GameObject bullet3 = Instantiate(PoisonBullet);
            bullet3.transform.position = transform.position + direction * 1.0f;
            bullet3.GetComponent<Rigidbody2D>().linearVelocity = directionRight * bulletspeed;
            bullet3.GetComponent<SpriteRenderer>().color = Color.green;

            GameObject bullet4 = Instantiate(NormalBullet);
            bullet4.transform.position = transform.position + direction * 1.25f;
            bullet4.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletspeed;
            bullet4.GetComponent<SpriteRenderer>().color = Color.red;

            GameObject bullet5 = Instantiate(NormalBullet);
            bullet5.transform.position = transform.position + direction * 1.25f;
            bullet5.GetComponent<Rigidbody2D>().linearVelocity = directionLeft * bulletspeed;
            bullet5.GetComponent<SpriteRenderer>().color = Color.red;

            GameObject bullet6 = Instantiate(NormalBullet);
            bullet6.transform.position = transform.position + direction * 1.25f;
            bullet6.GetComponent<Rigidbody2D>().linearVelocity = directionRight * bulletspeed;
            bullet6.GetComponent<SpriteRenderer>().color = Color.red;

        }
        
    }
    void Attack()
    {
        if (Cooldown <= Doomsday)
        {
            if (phase1)
            {
                rb.AddForce(Steering.Seek(gameObject, player.transform.position, moveSpeed));
            }
            else
            {
                rb.AddForce(Steering.Seek(gameObject, player.transform.position, moveSpeed + 1.5f));
            }
        }
    }

    Vector3 Avoid()
    {
        float distance = detectRadius * 0.5f;
        Vector3 dirRight = Quaternion.Euler(0.0f, 0.0f, -20.0f) * transform.up;
        Vector3 dirLeft = Quaternion.Euler(0.0f, 0.0f, 20.0f) * transform.up;
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, dirLeft, distance);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, dirRight, distance);

        Vector3 force = Vector3.zero;
        if (hitLeft && hitLeft.collider.CompareTag("Obstacle"))
        {
            force += Steering.Seek(gameObject, transform.right * distance, moveSpeed);
        }
        else if (hitRight && hitRight.collider.CompareTag("Obstacle"))
        {
            force += Steering.Seek(gameObject, -transform.right * distance, moveSpeed);
        }
        return force;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Boss_Health -= FindAnyObjectByType<Player>().Damage;

        }
    }
}

