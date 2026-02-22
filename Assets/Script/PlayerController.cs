using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 10f; // Player movement speed
    private Rigidbody2D rb; // Reference to the player's Rigidbody2D component
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to this GameObject
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // Get horizontal input
        float moveVertical = Input.GetAxis("Vertical"); // Get vertical input

        rb.linearVelocity = new Vector2(moveHorizontal * speed, moveVertical * speed); // Set the player's velocity based on input and speed
    }
}
