using UnityEngine;

public class CameraJAXEN : MonoBehaviour
{
    //UNUSED!!!
    //So the camera can move with the player but also not be affected by the players rotation. 
    //This went unused because theres no scenes that need it.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 5.0f;
        Vector3 direction = Vector3.zero;

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

        direction = direction.normalized;
        Vector3 Movement = direction * speed * Time.deltaTime;
        transform.position += Movement;
    }
}
