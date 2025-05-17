using UnityEngine;

public class Mouvement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player
    public float rotationSpeed = 75f; // Speed of rotation
   
    private Rigidbody rb; // Reference to the Rigidbody component>

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the player
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }


    void FixedUpdate()
    {
        float moveInput = Input.GetAxisRaw("Vertical"); // Get vertical input (W/S or Up/Down arrows)
        float turnInput = Input.GetAxisRaw("Horizontal"); // Get horizontal input (A/D or Left/Right arrows)

        Vector3 move = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime; // Calculate movement vector
        rb.MovePosition(rb.position + move); // Move the player using Rigidbody

        float rotation = turnInput * rotationSpeed * Time.fixedDeltaTime; // Calculate rotation angle
        Quaternion turnOffset = Quaternion.Euler(0, rotation, 0); // Create a rotation quaternion
        rb.MoveRotation(rb.rotation * turnOffset); // Rotate the player using Rigidbody
   
    }
}