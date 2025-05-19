using UnityEngine;

public class sky : MonoBehaviour
{
    public Texture2D skyTexture; // Drag your image into this in the inspector

    void Start()
    {
        // Create a plane
        GameObject skyPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        skyPlane.name = "SkyOverhead";

        // Position it at y = 1.6
        skyPlane.transform.position = new Vector3(0, 1.6f, 0);

        // Scale it so it's 31 x 31 units (Unity's plane is 10 x 10 by default)
        skyPlane.transform.localScale = new Vector3(3.1f, 1f, 3.1f);

        // Flip it upside-down so you can see the texture from below
        skyPlane.transform.rotation = Quaternion.Euler(180f, 0f, 0f);

        // Create and assign a material with your texture
        Material skyMaterial = new Material(Shader.Find("Standard"));
        skyMaterial.mainTexture = skyTexture;

        // Set tiling to 31 x 31 for repeating the texture
        skyMaterial.mainTextureScale = new Vector2(31f, 31f);

        // Apply the material to the plane
        skyPlane.GetComponent<Renderer>().material = skyMaterial;
    }
}
