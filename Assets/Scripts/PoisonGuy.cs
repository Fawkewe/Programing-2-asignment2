using UnityEngine;

public class PoisonGuy : MonoBehaviour
{

    public float Poison_Health = 2.0f;
    public float Poison_Damage = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        if (Poison_Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Poison_Health -= FindAnyObjectByType<Player>().Damage;
    
        }
    }
    private void OnDestroy()
    {
       FindAnyObjectByType<Game>().Enemy_Count -= 1;
    }
}
