using System.Collections.Generic; 
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject chestPrefab; // Assign your chest prefab in the Inspector

    void Start()
    {
        SpawnChestsAtRandomPositions();
    }

    void SpawnChestsAtRandomPositions()
    {
        var positions = VariablesGlobales.walkablePositions;
        var tresorArray = VariablesGlobales.Tresor;
        int chestsToSpawn = tresorArray.Length > 0 ? tresorArray[0] : 0;

        if (positions == null || positions.Count == 0)
        {
            Debug.LogWarning("No walkable positions available!");
            return;
        }

        if (chestsToSpawn > positions.Count)
        {
            Debug.LogWarning("Not enough walkable positions for all chests!");
            chestsToSpawn = positions.Count;
        }

        // Create a copy of positions to avoid duplicates
        var availablePositions = new List<Vector3>(positions);

        for (int i = 0; i < chestsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Vector3 spawnPosition = availablePositions[randomIndex];
            Instantiate(chestPrefab, spawnPosition, Quaternion.identity);
            Debug.Log($"Chest {i + 1} spawned at position: {spawnPosition}");
            availablePositions.RemoveAt(randomIndex); // Prevent duplicate spawns
        }
    }
}