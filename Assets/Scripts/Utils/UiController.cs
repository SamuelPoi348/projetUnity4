using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TMP_Text statusText;

   // public int score = 0;
   // public int level = 1;
   // public int wallOpeners = 3;

   // private float remainingTime = 60f; // Démarre à 60 secondes
    private bool timerRunning = true;

    void Update()
    {
        if (timerRunning && VariablesGlobales.time  > 0)
        {
            VariablesGlobales.time  -= Time.deltaTime;

            // Clamp à 0 pour éviter les valeurs négatives
            if (VariablesGlobales.time  < 0)
                VariablesGlobales.time  = 0;
        }

        int displayTime = Mathf.CeilToInt(VariablesGlobales.time ); // Arrondi vers le haut

        statusText.text = $"Score: {VariablesGlobales.score}    Time: {displayTime}s    Level: {VariablesGlobales.level}    Wall Openers: {VariablesGlobales.wallOpeners}";
    }

    // Facultatif : méthode pour arrêter le timer
    public void StopTimer()
    {
        timerRunning = false;
    }

    // Facultatif : méthode pour relancer ou réinitialiser
    public void ResetTimer(float time = 60f)
    {
        VariablesGlobales.time  = time;
        timerRunning = true;
    }
}
