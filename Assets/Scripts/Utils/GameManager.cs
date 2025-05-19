using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Add this line at the top

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject chestPrefab;
    public GameObject teleporterPrefab;
    public GameObject arrowPrefab; // Assign your arrow prefab in the Inspector

    public GameObject deathScreen; // Drag this from the Inspector
    public GameObject winScreen; // Drag this from the Inspector

    public GameObject teleReceiverPrefab;

    public Camera CameraJoueur;     // Player's normal view
    public Camera topDownCamera;    // Overhead view

    public GameObject player; // Assign this in the Inspector

    //Sons 
    public AudioSource audioSource; // Assign in Inspector
    public AudioClip SonGameOver;   // Assign in Inspector
    public AudioClip SonWin;   // Assign in Inspector
    public AudioClip SonNiveauSuivant;   // Assign in Inspector
    public AudioClip SonMort;
    public AudioClip debutNiveau;
    public AudioClip SonTeleport; // Assign in Inspector
    public AudioClip SonMonteMur;

    private bool cheatModeActive = false;
    private int topDownHiddenLayer;

    public int penaltyRate = 10;

    private float topDownTimeCounter = 0f;

    void Start()
    {
        SpawnChestsAtRandomPositions();
        SpawnTeleportersAtRandomPositions();
        SpawnTeleReceiversAtRandomPositions();

        SetTopDown(false); // Start in normal view
    }

    private bool gameEnded = false;
    void Update()
    {
        // Camera switch
        if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.PageUp)) && VariablesGlobales.score >= 10)
        {
            SetTopDown(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.PageDown))
        {
            SetTopDown(false);
        }

        // Cheat mode toggle (only in top-down view)
        if (VariablesGlobales.isTopDown && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space))
        {
            cheatModeActive = !cheatModeActive;
            UpdateTopDownHiddenObjects();
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

        // Reset cheat mode when leaving top-down view
        if (!topDown)
            cheatModeActive = false;

        UpdateTopDownHiddenObjects();
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

        // Call this after the chest is spawned and positionCoffre is set
        SpawnArrowsPointingToChest();
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



    void SpawnTeleReceiversAtRandomPositions()
    {
        var positions = VariablesGlobales.walkablePositions;
        var teleRecepteurArray = VariablesGlobales.teleRecepteurs;
        int niveau = VariablesGlobales.niveau;
        int receiversToSpawn = (teleRecepteurArray.Length > niveau - 1) ? teleRecepteurArray[niveau - 1] : 0;

        if (positions == null || positions.Count == 0)
        {
            Debug.LogWarning("No walkable positions available!");
            return;
        }

        if (receiversToSpawn > positions.Count)
        {
            Debug.LogWarning("Not enough walkable positions for all teleporter receivers!");
            receiversToSpawn = positions.Count;
        }

        var availablePositions = new List<Vector3>(positions);

        for (int i = 0; i < receiversToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Vector3 spawnPosition = availablePositions[randomIndex];
            Instantiate(teleReceiverPrefab, spawnPosition, Quaternion.identity);
            Debug.Log($"TeleReceiver {i + 1} spawned at position: {spawnPosition}");
            availablePositions.RemoveAt(randomIndex);
        }
    }


    void SpawnArrowsPointingToChest()
    {
        var positions = VariablesGlobales.walkablePositions;
        var flechesArray = VariablesGlobales.fleches;
        int niveau = VariablesGlobales.niveau;
        int arrowsToSpawn = (flechesArray.Length > niveau - 1) ? flechesArray[niveau - 1] : 0;

        if (positions == null || positions.Count == 0)
        {
            Debug.LogWarning("No walkable positions available!");
            return;
        }

        if (arrowsToSpawn > positions.Count)
        {
            Debug.LogWarning("Not enough walkable positions for all arrows!");
            arrowsToSpawn = positions.Count;
        }

        var availablePositions = new List<Vector3>(positions);

        for (int i = 0; i < arrowsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Vector3 spawnPosition = availablePositions[randomIndex] + Vector3.up * 1f; // Raise above ground

            Vector3 chestPosition = VariablesGlobales.positionCoffre;

            // Compute direction on horizontal (XZ) plane
            Vector3 flatDirection = chestPosition - spawnPosition;
            flatDirection.y = 0;
            flatDirection.Normalize();

            // Rotate arrow that normally points up (Y+) to look toward chest
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, flatDirection);

            GameObject arrow = Instantiate(arrowPrefab, spawnPosition, rotation);

            availablePositions.RemoveAt(randomIndex);
        }
    }

    void UpdateTopDownHiddenObjects()
    {
        // Find all objects in the TopDownHidden layer
GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        bool shouldShow = !VariablesGlobales.isTopDown || cheatModeActive;

        foreach (var obj in allObjects)
        {
            if (obj.layer == topDownHiddenLayer)
            {
                var renderers = obj.GetComponentsInChildren<Renderer>(true);
                foreach (var renderer in renderers)
                {
                    renderer.enabled = shouldShow;
                }
            }
        }
    }




    void Awake()
    {
        Instance = this;
        topDownHiddenLayer = LayerMask.NameToLayer("TopDownHidden");
    }

    public void PlayerWins()
    {
        Debug.Log("You win!");

        if (VariablesGlobales.level == 10)
        {
            if (winScreen != null)
            {
                audioSource.PlayOneShot(SonWin); // plays without interrupting existing sounds
                winScreen.SetActive(true);
                Time.timeScale = 0f; // Optional: pause game
            }
        }
        else
        {
             audioSource.PlayOneShot(SonNiveauSuivant); // plays without interrupting existing sounds
            VariablesGlobales.level += 1;
            VariablesGlobales.score += (int)(10 * ((int)VariablesGlobales.time));
            VariablesGlobales.time = 60; // Reset time
            VariablesGlobales.niveau += 1;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads the current scene


        }

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe event
        Time.timeScale = 1f; // Ensure time is running so audio plays
        audioSource.PlayOneShot(SonNiveauSuivant); // Play the level up sound
    }
    public void PlayerLoses()
    {
        Debug.Log("You lose!");
        if (VariablesGlobales.score < 200)
        {
            // Show death screen UI
            if (deathScreen != null)
            {
                deathScreen.SetActive(true);
                Time.timeScale = 0f; // Optional: pause game
                audioSource.PlayOneShot(SonGameOver); // plays without interrupting existing sounds
            }
            else
            {
                Debug.LogWarning("Death screen not assigned in the inspector.");

            }
        }
        else
        {
            VariablesGlobales.score -= 200; // Deduct points
            audioSource.PlayOneShot(sonMort); // plays without interrupting existing sounds
            // Do NOT reset VariablesGlobales.niveau here
            VariablesGlobales.time = 60; // Reset time
            VariablesGlobales.wallOpeners = VariablesGlobales.ouvreurMur[VariablesGlobales.niveau - 1]; // Reset wall openers
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
        }

    }

}
