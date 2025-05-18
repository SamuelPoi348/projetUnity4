using UnityEngine;

public class TriangleOnCapsule : MonoBehaviour
{
    public float heightOffset = 1.5f;  // Vertical position above the capsule

    void Start()
    {
        GameObject capsule = gameObject;

        GameObject triangle = new GameObject("Triangle");
        triangle.transform.SetParent(capsule.transform);

        MeshFilter meshFilter = triangle.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = triangle.AddComponent<MeshRenderer>();

        // Red material
        Material triangleMaterial = new Material(Shader.Find("Standard"));
        triangleMaterial.color = Color.red;
        meshRenderer.material = triangleMaterial;

        // Create triangle mesh
        meshFilter.mesh = CreateTriangleMesh();

        // Position above the capsule
        triangle.transform.localPosition = new Vector3(0f, capsule.transform.localScale.y / 2 + heightOffset, 0f);

        // Rotate flat on XZ plane and add 180° twist to reverse its direction
        triangle.transform.localEulerAngles = new Vector3(-90f, 180f, 0f);

        // Scale it down to 0.5
        triangle.transform.localScale = new Vector3(0.75f, 0.75f, 0.9f);
    }

    Mesh CreateTriangleMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[3]
        {
            new Vector3(-1, -0.4f, 0),                    // Left
            new Vector3(1, -0.4f, 0),                     // Right
            new Vector3(0f, Mathf.Sqrt(3) / 2, 0)     // Top (center)
        };

        int[] triangles = new int[3] { 0, 1, 2 };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}
