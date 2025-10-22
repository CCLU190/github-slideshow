using UnityEngine;

/// <summary>
/// Controls the transparent torso model that contains the organs
/// Size: 60cm height x 30cm width x 25cm depth
/// </summary>
public class TransparentTorsoController : MonoBehaviour
{
    [Header("Torso Dimensions (in meters)")]
    [SerializeField] private Vector3 dimensions = new Vector3(0.3f, 0.6f, 0.25f);  // 30cm x 60cm x 25cm

    [Header("Transparency Settings")]
    [SerializeField, Range(0f, 1f)] private float transparency = 0.3f;

    [Header("Visual Settings")]
    [SerializeField] private Color torsoColor = new Color(0.8f, 0.9f, 1f, 0.3f);  // Light blue, transparent

    private GameObject torsoMesh;
    private Material torsoMaterial;

    private void Awake()
    {
        CreateTorsoModel();
    }

    /// <summary>
    /// Create the transparent torso mesh
    /// </summary>
    private void CreateTorsoModel()
    {
        // Create a box that represents the torso cavity
        torsoMesh = GameObject.CreatePrimitive(PrimitiveType.Cube);
        torsoMesh.name = "TorsoMesh";
        torsoMesh.transform.parent = transform;
        torsoMesh.transform.localPosition = Vector3.zero;
        torsoMesh.transform.localRotation = Quaternion.identity;
        torsoMesh.transform.localScale = dimensions;

        // Remove the collider as we don't want to interact with the torso itself
        Collider collider = torsoMesh.GetComponent<Collider>();
        if (collider != null)
        {
            Destroy(collider);
        }

        // Create transparent material
        CreateTransparentMaterial();

        // Create wireframe outline for better visibility
        CreateWireframeOutline();
    }

    /// <summary>
    /// Create a transparent material for the torso
    /// </summary>
    private void CreateTransparentMaterial()
    {
        Renderer renderer = torsoMesh.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Create a transparent material using Standard shader
            torsoMaterial = new Material(Shader.Find("Standard"));

            // Set rendering mode to transparent
            torsoMaterial.SetFloat("_Mode", 3);  // Transparent mode
            torsoMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            torsoMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            torsoMaterial.SetInt("_ZWrite", 0);
            torsoMaterial.DisableKeyword("_ALPHATEST_ON");
            torsoMaterial.EnableKeyword("_ALPHABLEND_ON");
            torsoMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            torsoMaterial.renderQueue = 3000;

            // Set color with transparency
            torsoColor.a = transparency;
            torsoMaterial.color = torsoColor;

            renderer.material = torsoMaterial;
        }
    }

    /// <summary>
    /// Create wireframe outline for better visibility
    /// </summary>
    private void CreateWireframeOutline()
    {
        // Create edge lines to make the torso boundaries more visible
        GameObject outline = new GameObject("TorsoOutline");
        outline.transform.parent = transform;
        outline.transform.localPosition = Vector3.zero;
        outline.transform.localRotation = Quaternion.identity;

        // You can add LineRenderer components here to draw edges
        // For simplicity, we'll use a slightly larger cube with only edges visible
        GameObject outlineBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        outlineBox.transform.parent = outline.transform;
        outlineBox.transform.localPosition = Vector3.zero;
        outlineBox.transform.localScale = dimensions * 1.01f;  // Slightly larger

        // Remove collider
        Collider outlineCollider = outlineBox.GetComponent<Collider>();
        if (outlineCollider != null)
        {
            Destroy(outlineCollider);
        }

        // Create wireframe material
        Renderer outlineRenderer = outlineBox.GetComponent<Renderer>();
        if (outlineRenderer != null)
        {
            Material wireMaterial = new Material(Shader.Find("Standard"));
            wireMaterial.color = new Color(0.5f, 0.7f, 1f, 0.5f);  // Light blue
            wireMaterial.SetFloat("_Mode", 3);
            wireMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            wireMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            wireMaterial.SetInt("_ZWrite", 0);
            wireMaterial.EnableKeyword("_ALPHABLEND_ON");
            wireMaterial.renderQueue = 3000;

            outlineRenderer.material = wireMaterial;
        }
    }

    /// <summary>
    /// Set transparency level
    /// </summary>
    public void SetTransparency(float alpha)
    {
        transparency = Mathf.Clamp01(alpha);
        if (torsoMaterial != null)
        {
            Color color = torsoMaterial.color;
            color.a = transparency;
            torsoMaterial.color = color;
        }
    }

    /// <summary>
    /// Get the dimensions of the torso
    /// </summary>
    public Vector3 GetDimensions()
    {
        return dimensions;
    }

    /// <summary>
    /// Set custom dimensions
    /// </summary>
    public void SetDimensions(Vector3 newDimensions)
    {
        dimensions = newDimensions;
        if (torsoMesh != null)
        {
            torsoMesh.transform.localScale = dimensions;
        }
    }
}
