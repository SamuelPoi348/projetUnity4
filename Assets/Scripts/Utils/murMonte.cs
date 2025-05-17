using UnityEngine;

public class TriggerWall : MonoBehaviour
{
    public GameObject wall; // Assign the wall GameObject in the Inspector
    public Transform player; // Assign the player Transform in the Inspector
    public bool bin = false; // Use 'bool' instead of 'boolean'

    void Update()
    {
        if (player.position.z >= 2.5f && bin == false)
        {
            wall.transform.position = new Vector3(
                wall.transform.position.x, 
                0.5f, 
                wall.transform.position.z
            );
            bin = true; // Ensure this block only runs once
        }
    }
}

