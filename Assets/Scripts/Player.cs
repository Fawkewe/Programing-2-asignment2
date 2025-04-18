using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float Player_Health;
    public float MaxHealth;
    public float speed = 5.0f;
    public float bulletSpeed = 5f;
    public float Damage;
    public int Ammo = 21;
    public int MaxAmmo = 21;
    bool isReloading;
    float ReloadTime;
    float Poison_damage;
   

    bool isPoisoned;
    public GameObject BulletObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;
        Vector3 mouse = Input.mousePosition;
        Vector3 mouseDirection = (mouse - transform.position).normalized;
      

        if (Input.GetKey(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction += Vector3.up;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction += Vector3.down;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction += Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Ammo > 0 && isReloading == false)
            {
                ShootRifle(mouseDirection);
                Ammo -= 1;
                Debug.Log(Ammo);
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && isReloading == false || Ammo == 0)
        {
            Debug.Log("Reloading");
            isReloading = true;
            if (isReloading)
            {
                ReloadTime += Time.deltaTime;
            }
            else
            {
                ReloadTime = 0;
            }
            if (ReloadTime >= 2.0f)
            {
                Ammo = MaxAmmo;
                ReloadTime = 0;
                Debug.Log(Ammo);
                isReloading = false;
                Debug.Log("The Gun is Reloaded");
            }
        }
        else if (/*Input.GetKeyDown(KeyCode.R) && */ isReloading == true)
        {
            Debug.Log("is already reloading");
        }
        else
        {

        }
        direction = direction.normalized;
        Vector3 Movement = direction * speed * Time.deltaTime;
        transform.position += Movement;

        // while(ReloadTime != 0)
        // {
        //     isReloading = true;
        // }

        if (Player_Health <= 0)
        {
            SceneManager.LoadScene("Level 1");
        }
        if (Player_Health > MaxHealth)
        {
            Player_Health = MaxHealth;
        }
        if (isPoisoned == true)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            Poison_damage += Time.deltaTime / 2000.0f;
            Player_Health -= Poison_damage;
        }
        void ShootRifle(Vector3 direction)
        {
            GameObject bullet = Instantiate(BulletObject);
            bullet.transform.position = transform.position + direction * 0.75f;
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;
            Destroy(bullet, 5.0f);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pistol")
        {
            Player_Health -= FindAnyObjectByType<PistolGuy>().Pistol_Damage;
        }
        if (collision.gameObject.tag == "Poison")
        {
            Player_Health -= FindAnyObjectByType<PoisonGuy>().Poison_Damage;
            isPoisoned = true;
 
        }
        if (collision.gameObject.tag == "Toxic")
        {
            Player_Health -= FindAnyObjectByType<Toxicguy>().Toxic_Damage;
            //Player_Health *= FindAnyObjectByType<Toxicguy>().Toxic_Damage;
        }
    }
}


