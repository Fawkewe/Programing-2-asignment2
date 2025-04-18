using UnityEngine;

public class PistolGuy : MonoBehaviour
{

    public float Pistol_Health = 3.0f;
    public float Pistol_Damage = 1.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Pistol_Health >= 0) //It was destroying on spawn consistently and i was wondering why. How? ARE YOU FUCKING KIDDING ME THIS WAS WHY. 
         *
        {
            Destroy(gameObject); and yeah its why the inheritance script didn't work either im just too lazy to fix it atp. This genuinely drove me mad. 
. 

        //This is why i started laughing maniacally in case you remember or care. 

        Then get a kick out of this ...it still didn't work. I was like "uh oh. why isn't it working" You get one guess as to why?


        I DIDNT SAVE THE SCRIPT! hahaahahah. 

        }*/
        if (Pistol_Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Pistol_Health -= FindAnyObjectByType<Player>().Damage;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            collision = null;
        }
    }
    private void OnDestroy()
    {
       FindAnyObjectByType<Game>().Enemy_Count -= 1;
    }
}
