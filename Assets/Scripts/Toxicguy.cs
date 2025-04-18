using UnityEngine;

public class Toxicguy : MonoBehaviour
{
    //Sorry if the name is confusing this was original based off of a battle cats status affect that reduces health a certain percentage.
    //The affect itself actually did work but when the player was killed by it crashed the scene instead of taking the player to level 1. 
    //this has been changed to a Suicide Bomber attack.
    //also it might be confusing to have a script called Toxic AND poison but they are very different attacks, but the OG name of the affect is Toxic so i left it that way.
    public float Toxic_Health = 2.0f;
    public float Toxic_Damage = 30f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.magenta;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Toxic_Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Toxic_Health -= FindAnyObjectByType<Player>().Damage;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        FindAnyObjectByType<Game>().Enemy_Count -= 1;
    }
}
