using System.Threading;
using System;
using Unity.VisualScripting;
using UnityEngine;
//UNUSED MORE DETAIL BELOW! This comment is so you do not waste time looking here. 
//i tried but this didn't work. It just didn't save the variables properly (Enemies had no health, bullets had no speed etc).
//Probaly made a simple mistake...this is mostly your code but i did try to modify it a bit  to fit this model. 
//i Decided better just try what i know and make multiple scripts for each. I know that is an inefficient way of doing it. I hate giving up like  that but atleast it works right?
//Only submitted this because i didn't know if i needed to in order for the submission to be accepted.
public abstract class EnemyData : MonoBehaviour
{
    public float Enemy_Health;
    public float Enemy_Speed; //= 5.0f;
    public float Enemy_Damage;
    public float Enemy_Cooldown;
    public float Enemy_Reset;
}
public class ThePistolDude : EnemyData
{

}
public class Poisoner : EnemyData
{

}
public class Enemy3 : EnemyData
{

}
public class Enemy : MonoBehaviour
{
    EnemyData Pistol = new ThePistolDude();
    EnemyData Poison = new Poisoner();
    EnemyData Enemy3 = new Enemy3();
    public EnemyData EnemyData = null;
    public int EnemyType;

    float turnSpeed = 250.0f * Mathf.Deg2Rad;
    float detectRadius = 5.0f;


    Rigidbody2D rb;
    [SerializeField]
    GameObject player;
    public GameObject[] waypoints;
    int curr = 0;
    int next = 1;
    float ahead = 2.0f;
    public GameObject bullet;


    enum State
    {
        PATROL,
        ATTACK
    }

    State state = State.PATROL;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Pistol.Enemy_Health = 5;
        Pistol.Enemy_Speed = 5.0f;
        Pistol.Enemy_Damage = 1.0f;
        Pistol.Enemy_Cooldown = 2.0f;
        Pistol.Enemy_Reset = 2.0f;

        Poison.Enemy_Health = 2;
        Poison.Enemy_Speed = 2.0f;
        Poison.Enemy_Damage = 0.0f;
        Poison.Enemy_Cooldown = 2.0f;
        Poison.Enemy_Reset = 2.0f;

        Enemy3.Enemy_Health = 2;
        Enemy3.Enemy_Speed = 5.0f;
        Enemy3.Enemy_Damage = 10.0f;
        Enemy3.Enemy_Cooldown = 2.0f;
        Enemy3.Enemy_Reset = 2.0f;


        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
      switch(EnemyType)
        {
            case 0:
                EnemyData = Pistol;
                break;
            case 1:
                EnemyData = Poison;
            break;
            case 2:
                EnemyData = Enemy3;
             break;

        }
      if (EnemyData.Enemy_Health >= 0)
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
      //  Debug.DrawLine(transform.position, transform.position + transform.up * 5.0f, Color.green);
        EnemyData.Enemy_Cooldown -= Time.deltaTime;

    }
    void UpdateState()
    {
        Vector3 toPlayer = player.transform.position - transform.position.normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer, detectRadius);
        bool playerHit = hit && hit.collider.CompareTag("Player");
        state = playerHit ? State.ATTACK : State.PATROL;
    }
    void Patrol()
    {
      rb.AddForce(Steering.FollowLine(gameObject, waypoints, ref curr, ref next, EnemyData.Enemy_Speed, ahead));
    }
    void Shot(Vector3 direction)
    {

        GameObject Bullet = Instantiate(bullet);
        bullet.transform.position = transform.position + direction * 0.75f;
        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * EnemyData.Enemy_Speed;
        bullet.GetComponent<SpriteRenderer>().color = Color.red;
        //Destroy(bullet, 1.0f);
    }
    void Attack()
    {


        if (EnemyData.Enemy_Cooldown <= 0) ;
        {
            Shot(player.transform.position);
            EnemyData.Enemy_Cooldown = EnemyData.Enemy_Reset;
            Debug.Log("Check");
        }
        rb.AddForce(Steering.Seek(gameObject, player.transform.position, EnemyData.Enemy_Speed));
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
           force += Steering.Seek(gameObject, transform.right * distance, EnemyData.Enemy_Speed);
       }
      else if (hitRight && hitRight.collider.CompareTag("Obstacle"))
      {
         force += Steering.Seek(gameObject, -transform.right * distance, EnemyData.Enemy_Speed);
      }
      return force;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.tag == "PlayerBullet")
        {
            EnemyData.Enemy_Health -= FindAnyObjectByType<Player>().Damage;
        }
    }
 private void OnDestroy()
    {
        FindAnyObjectByType<Game>().Enemy_Count -= 1;
    }
}
