using UnityEngine;

public class RenameToMur : MonoBehaviour
{
    [ContextMenu("Rename Children To Mur1-Mur31")]
    void RenameChildren()
    {
        int count = 1;
        foreach (Transform child in transform)
        {
            child.name = "Mur" + count;
            count++;
        }
    }
}
