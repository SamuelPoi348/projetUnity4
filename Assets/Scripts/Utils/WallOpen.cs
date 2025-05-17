using UnityEngine;

public class WallOpen : MonoBehaviour
{

    public float distanceInteraction = 1f;
    public float dureeGlissement = 1.0f;
    public float niveauSol = -0.1999f;
    public LayerMask coucheOuvrable;

    // Update is called once per frame
    void Update()
    {
        // Vérifie si la touche Espace est pressée
        if (Input.GetKeyDown(KeyCode.Space) && VariablesGlobales.wallOpeners > 0)
        {
            VariablesGlobales.score -=50;
            VariablesGlobales.wallOpeners--; // Décrémente le nombre d'ouvreurs de mur
            // Lance un rayon depuis la position du joueur vers l'avant
            Ray rayon = new Ray(transform.position, transform.forward);
            RaycastHit touche;

            // Vérifie si on touche un objet cassable dans la distance définie
            if (Physics.Raycast(rayon, out touche, distanceInteraction, coucheOuvrable))
            {
                // Démarre l’animation de glissement vers le bas du cube touché
                StartCoroutine(FaireGlisserCube(touche.transform));
            }
        }
    }

    private System.Collections.IEnumerator FaireGlisserCube(Transform cube)
    {
        Vector3 positionInitiale = cube.position;
        Vector3 positionFinale = new Vector3(positionInitiale.x, niveauSol, positionInitiale.z);
        float tempsEcoule = 0f;

        while (tempsEcoule < dureeGlissement)
        {
            cube.position = Vector3.Lerp(positionInitiale, positionFinale, tempsEcoule / dureeGlissement);
            tempsEcoule += Time.deltaTime;
            yield return null;
        }

        // S'assure que la position finale est bien atteinte
        cube.position = positionFinale;

        VariablesGlobales.murBaisser++; // Met à jour la variable globale pour indiquer que le mur a été abaissé
        Debug.Log("Nombre de murs abaissés : " + VariablesGlobales.murBaisser);
    }
}