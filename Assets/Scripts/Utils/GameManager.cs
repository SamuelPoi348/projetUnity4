using System.Collections.Generic; 
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject chestPrefab; // Assign your chest prefab in the Inspector
    public GameObject teleporterPrefab; // Assign your teleporter prefab in the Inspector

    void Start()
    {
        SpawnChestsAtRandomPositions();
        SpawnTeleportersAtRandomPositions();
    }

    void SpawnChestsAtRandomPositions()
    {
        var positions = VariablesGlobales.walkablePositions;
        var tresorArray = VariablesGlobales.Tresor;
        int niveau = VariablesGlobales.niveau;
        int chestsToSpawn = (tresorArray.Length > niveau - 1) ? tresorArray[niveau - 1] : 0;

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

        var availablePositions = new List<Vector3>(positions);

        for (int i = 0; i < chestsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Vector3 spawnPosition = availablePositions[randomIndex];
            Instantiate(chestPrefab, spawnPosition, Quaternion.identity);
            Debug.Log($"Chest {i + 1} spawned at position: {spawnPosition}");
            availablePositions.RemoveAt(randomIndex);
            VariablesGlobales.positionCoffre = spawnPosition;
            Debug.Log($"Position coffre: {VariablesGlobales.positionCoffre}");
        }
    }

    void SpawnTeleportersAtRandomPositions()
    {
        var positions = VariablesGlobales.walkablePositions;
        var teleArray = VariablesGlobales.teleTransporteurs;
        int niveau = VariablesGlobales.niveau;
        int teleportersToSpawn = (teleArray.Length > niveau - 1) ? teleArray[niveau - 1] : 0;

        if (positions == null || positions.Count == 0)
        {
            Debug.LogWarning("No walkable positions available!");
            return;
        }

        if (teleportersToSpawn > positions.Count)
        {
            Debug.LogWarning("Not enough walkable positions for all teleporters!");
            teleportersToSpawn = positions.Count;
        }

        var availablePositions = new List<Vector3>(positions);

        for (int i = 0; i < teleportersToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Vector3 spawnPosition = availablePositions[randomIndex];
            Instantiate(teleporterPrefab, spawnPosition, Quaternion.identity);
            Debug.Log($"Teleporter {i + 1} spawned at position: {spawnPosition}");
            availablePositions.RemoveAt(randomIndex);
        }
    }
}