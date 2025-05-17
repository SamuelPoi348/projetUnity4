using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 100f;

    private Vector2 moveInput;
    private float turnInput;

    void Update()
    {
        // Get movement input using the new Input System
        moveInput = Keyboard.current.wKey.isPressed ? new Vector2(0, 1) :
                    Keyboard.current.sKey.isPressed ? new Vector2(0, -1) :
                    new Vector2(0, 0);

        // Get turn input for left and right rotation
        turnInput = (Keyboard.current.dKey.isPressed ? 1f : 0f) - (Keyboard.current.aKey.isPressed ? 1f : 0f);

        // Movement: move forward or backward
        transform.Translate(Vector3.forward * moveInput.y * moveSpeed * Time.deltaTime);

        // Rotation: turn left or right
        transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.deltaTime);
    }
}
