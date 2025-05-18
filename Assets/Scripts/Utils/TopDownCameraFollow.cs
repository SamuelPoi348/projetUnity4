using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    public Transform target;         // The player or object to follow
    public Vector3 offset = new Vector3(0f, 35f, 0f); // Offset from the target

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = offset;
            transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Always look straight down
        }
    }
}
