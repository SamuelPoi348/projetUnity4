using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject chestPrefab;
    public GameObject teleporterPrefab;

    public Camera CameraJoueur;     // Player's normal view
    public Camera topDownCamera;    // Overhead view

    public GameObject player; // Assign this in the Inspector

    public int penaltyRate = 10;

    private float topDownTimeCounter = 0f;

    void Start()
    {
        SpawnChestsAtRandomPositions();
        SpawnTeleportersAtRandomPositions();

        SetTopDown(false); // Start in normal view
    }

    private bool gameEnded = false;
    void Update()
    {
        // Camera switch
        if (Input.GetKeyDown(KeyCode.Alpha1) && VariablesGlobales.score >= 10)
        {
            SetTopDown(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetTopDown(false);
        }

        // Point penalty while in top-down view
        if (VariablesGlobales.isTopDown)
        {
            topDownTimeCounter += Time.deltaTime;

            if (topDownTimeCounter >= 1f)
            {
                int pointsToSubtract = Mathf.FloorToInt(topDownTimeCounter) * penaltyRate;
                VariablesGlobales.score -= pointsToSubtract;
                VariablesGlobales.score = Mathf.Max(VariablesGlobales.score, 0);
                topDownTimeCounter -= Mathf.Floor(topDownTimeCounter); // keep leftover time
                if (VariablesGlobales.score <= 0)
                {
                    SetTopDown(false); // Switch back to normal view if score is zero
                }
            }
        }

        if (VariablesGlobales.time <= 0 && !gameEnded)
        {
            gameEnded = true;
            PlayerLoses();
        }

        // Optional: display for debugging
        // Debug.Log("Current Points: " + VariablesGlobales.score);
    }

    void SetTopDown(bool topDown)
    {
        VariablesGlobales.isTopDown = topDown;
        topDownCamera.gameObject.SetActive(topDown);  // Enable top-down camera
        CameraJoueur.gameObject.SetActive(!topDown);  // Disable normal camera

        if (player != null)
        {
            // Hide all MeshRenderers on CorpsJoueur only, always show the triangle
            foreach (var renderer in player.GetComponentsInChildren<MeshRenderer>(true))
            {
                if (renderer.gameObject.name == "Triangle")
                {
                    renderer.enabled = true; // Always show the triangle
                }
                else if (renderer.gameObject.name == "CorpsJoueur")
                {
                    renderer.enabled = !topDown; // Hide CorpsJoueur in top-down view
                }
            }
        }
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



    void Awake()
    {
        Instance = this;
    }

    public void PlayerWins()
    {
        Debug.Log("You win!");
        // Show win UI, stop player movement, etc.
    }

    public void PlayerLoses()
    {
        Debug.Log("You lose!");
        // Show lose UI, stop player movement, etc.
    }


}
