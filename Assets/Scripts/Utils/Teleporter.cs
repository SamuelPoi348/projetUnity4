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
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // If using CharacterController, disable before teleporting
            CharacterController cc = other.GetComponentInParent<CharacterController>();
            if (cc != null) cc.enabled = false;

            // Move the root player object
            Transform playerRoot = other.transform.root;
            playerRoot.position = randomReceiver.transform.position + Vector3.up * yOffset;

            if (cc != null) cc.enabled = true;

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.linearVelocity = Vector3.zero;
            }

            Debug.Log("Player teleported to: " + randomReceiver.transform.position);
        }
    }
}