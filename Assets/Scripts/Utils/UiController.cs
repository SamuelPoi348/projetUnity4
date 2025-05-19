using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public TMP_Text statusText;

    private bool timerRunning = false;

    void Update()
    {
        // Start timer when an input is made (e.g., pressing a key)
       if (!timerRunning && (
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.PageUp) ||
            Input.GetKeyDown(KeyCode.PageDown) ||
            Input.GetKeyDown(KeyCode.Alpha1) ||
            Input.GetKeyDown(KeyCode.Alpha2) ||
            Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.RightArrow)
        ))
        {
            ResetTimer(60f); // Reset timer to 60 seconds when any of the keys are detected
        }

        // If the timer is running, decrement the time
        if (timerRunning && VariablesGlobales.time > 0)
        {
            VariablesGlobales.time -= Time.deltaTime;

            // Clamp time to 0 to avoid negative values
            if (VariablesGlobales.time < 0)
                VariablesGlobales.time = 0;
        }

        // Display time as an integer (rounded up)
        int displayTime = Mathf.CeilToInt(VariablesGlobales.time);

        // Update the UI text with the current status
        statusText.text = $"Score: {VariablesGlobales.score}    Time: {displayTime}s    Level: {VariablesGlobales.level}    Wall Openers: {VariablesGlobales.wallOpeners}";
    }

    // Method to stop the timer
    public void StopTimer()
    {
        timerRunning = false;
    }

    // Method to reset the timer (and start it again)
    public void ResetTimer(float time = 60f)
    {
        VariablesGlobales.time = time;
        timerRunning = true;
    }
}
