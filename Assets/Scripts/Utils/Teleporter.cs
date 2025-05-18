using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject[] receivers = GameObject.FindGameObjectsWithTag("TeleReceiver");
            if (receivers.Length == 0) return;

            GameObject randomReceiver = receivers[Random.Range(0, receivers.Length)];

            // Get the player's collider height for a proper offset
            float yOffset = 1f;
            Collider playerCollider = other.GetComponent<Collider>();
            if (playerCollider != null)
            {
                yOffset = playerCollider.bounds.extents.y + 0.1f; // Half height + small buffer
            }

            // Temporarily disable Rigidbody to avoid physics glitches
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            other.transform.position = randomReceiver.transform.position + Vector3.up * yOffset;

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.linearVelocity = Vector3.zero; // Reset velocity after teleport
            }

            Debug.Log("Player teleported to: " + randomReceiver.transform.position);
        }
    }
}