using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Pistol")
        {
            if (collision.gameObject.CompareTag("Pistol") || collision.gameObject.CompareTag("Boss"))
            {
                collision = null;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        if (tag == "Poison")
        {
            if (collision.gameObject.CompareTag("Poison") || collision.gameObject.CompareTag("Boss"))
            {
                collision = null;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        if (tag == "Toxic")
        {
            if (collision.gameObject.CompareTag("Toxic"))
            {
                collision = null;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        if (tag == "PlayerBullet")
        {
            Destroy(gameObject);
        }
    }
}
