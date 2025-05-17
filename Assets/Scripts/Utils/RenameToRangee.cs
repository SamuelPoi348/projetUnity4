using UnityEngine;

public class RenameToRangee : MonoBehaviour
{
    [ContextMenu("Rename Children To Rangee1-Rangee31")]
    void RenameChildren()
    {
        int count = 1;
        foreach (Transform child in transform)
        {
            child.name = "Rangee" + count;
            count++;
        }
    }
}
