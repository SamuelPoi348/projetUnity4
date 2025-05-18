using UnityEngine;
using System.Collections.Generic;

public class WallOpen : MonoBehaviour
{
    public float distanceInteraction = 1f;
    public float dureeGlissement = 1.0f;
    public float niveauSol = -0.1999f;
    public LayerMask coucheOuvrable;

    // Holds the set of walls currently being lowered
    private HashSet<Transform> mursEnCours = new HashSet<Transform>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && VariablesGlobales.wallOpeners > 0)
        {
            Ray rayon = new Ray(transform.position, transform.forward);
            RaycastHit touche;

            if (Physics.Raycast(rayon, out touche, distanceInteraction, coucheOuvrable))
            {
                Transform cible = touche.transform;

                // Prevent triggering the same wall again
                if (!mursEnCours.Contains(cible))
                {
                    mursEnCours.Add(cible); // Mark this wall as "in progress"
                    StartCoroutine(FaireGlisserCube(cible));
                }
            }
        }
    }

    private System.Collections.IEnumerator FaireGlisserCube(Transform cube)
    {
        VariablesGlobales.wallOpeners--;
        VariablesGlobales.score -= 50;

        Vector3 positionInitiale = cube.position;
        Vector3 positionFinale = new Vector3(positionInitiale.x, niveauSol, positionInitiale.z);
        float tempsEcoule = 0f;

        while (tempsEcoule < dureeGlissement)
        {
            cube.position = Vector3.Lerp(positionInitiale, positionFinale, tempsEcoule / dureeGlissement);
            tempsEcoule += Time.deltaTime;
            yield return null;
        }

        cube.position = positionFinale;

        VariablesGlobales.murBaisser++;
        Debug.Log("Nombre de murs abaissés : " + VariablesGlobales.murBaisser);

        // Mark this wall as finished
        mursEnCours.Remove(cube);
    }
}
