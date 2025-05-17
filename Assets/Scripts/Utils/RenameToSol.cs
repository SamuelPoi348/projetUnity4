using UnityEngine;

public class RenameToSol : MonoBehaviour
{
    [ContextMenu("Rename Children To Sol1-Sol31")]
    void RenameChildren()
    {
        int count = 1;
        foreach (Transform child in transform)
        {
            child.name = "Sol" + count;
            count++;
        }
    }
}
