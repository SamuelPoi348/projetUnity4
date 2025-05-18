using UnityEngine;
using System.Collections.Generic;

public static class VariablesGlobales
{

    // niveau courant
    public static int niveau = 10;

    // Arrays for items
    public static int[] Tresor = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    public static int[] ouvreurMur = new int[] { 4, 4, 3, 3, 2, 2, 1, 1, 0, 0 };
    public static int[] fleches = new int[] { 18, 16, 14, 12, 10, 8, 6, 4, 2, 0 };
    public static int[] teleTransporteurs = new int[] { 0, 1, 1, 2, 2, 3, 3, 4, 4, 5 };
    public static int[] teleRecepteurs = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    // Array for wall openers
    public static List<Vector3> walkablePositions = new List<Vector3>();

    // Score and map variables
    public static int level = niveau;
    public static int wallOpeners = 4;
    public static float time = 60f;
    public static int score = 300;
    public static int murBaisser = 0;
    public static bool isTopDown = false;

    //position coffre
    public static Vector3 positionCoffre = new Vector3(0, 0, 0);
}
