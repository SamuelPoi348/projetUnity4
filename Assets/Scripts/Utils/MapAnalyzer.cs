using UnityEngine;
using System.Collections.Generic;

public class MapAnalyzer : MonoBehaviour
{
    public Vector3 floorCenter = Vector3.zero;
    public int gridSize = 31;
    public float tileSize = 1f;
    public LayerMask wallLayer;
    public LayerMask indestructibleWallLayer;


    void Start()
    {
        AnalyzeFloor();
    }

    void AnalyzeFloor()
{
    VariablesGlobales.walkablePositions.Clear(); // Reset in case you re-run

    Vector3 bottomLeft = floorCenter - new Vector3(gridSize / 2f, 0f, gridSize / 2f);

    // Define the spawn zone positions to exclude
    HashSet<Vector2Int> spawnZone = new HashSet<Vector2Int>
    {
        new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(-1, 1),
        new Vector2Int(0, -1),  new Vector2Int(0, 0),  new Vector2Int(0, 1),
        new Vector2Int(1, -1),  new Vector2Int(1, 0),  new Vector2Int(1, 1)
    };

    for (int x = 0; x < gridSize; x++)
    {
        for (int z = 0; z < gridSize; z++)
        {
            Vector3 tileCenter = bottomLeft + new Vector3(x + 0.5f, 0f, z + 0.5f);

            // Convert to int coordinates relative to floorCenter
            int relX = Mathf.RoundToInt(tileCenter.x - floorCenter.x);
            int relZ = Mathf.RoundToInt(tileCenter.z - floorCenter.z);

            // Exclude spawn zone
            if (spawnZone.Contains(new Vector2Int(relX, relZ)))
                continue;

            // Check for both wall and indestructible wall
            bool hasWall = Physics.CheckSphere(tileCenter + Vector3.up * 0.5f, 0.4f, wallLayer);
            bool hasIndestructibleWall = Physics.CheckSphere(tileCenter + Vector3.up * 0.5f, 0.4f, indestructibleWallLayer);

            if (!hasWall && !hasIndestructibleWall)
            {
                VariablesGlobales.walkablePositions.Add(tileCenter);
            }
        }
    }

    Debug.Log($"Total walkable tiles: {VariablesGlobales.walkablePositions.Count}");
}

    // Optional: Draw green wire boxes on walkable positions
    void OnDrawGizmosSelected()
    {
        if (VariablesGlobales.walkablePositions != null)
        {
            Gizmos.color = Color.green;
            foreach (var pos in VariablesGlobales.walkablePositions)
            {
                Gizmos.DrawWireCube(pos + Vector3.up * 0.01f, new Vector3(1, 0.01f, 1));
            }
        }
    }
}
