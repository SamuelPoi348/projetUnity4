using UnityEngine;

public class Chest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
{
    Debug.Log("Chest trigger entered by: " + other.name);
    if (other.CompareTag("Player"))
    {
        Debug.Log("Player touched the chest!");
        GameManager.Instance.PlayerWins();
        Destroy(gameObject);
    }
}
}