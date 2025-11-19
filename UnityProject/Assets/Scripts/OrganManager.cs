using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all organs in the game - their positions, states, and initialization
/// </summary>
public class OrganManager : MonoBehaviour
{
    [Header("Organ Settings")]
    [SerializeField] private List<OrganData> organs = new List<OrganData>();

    [Header("Scatter Settings")]
    [SerializeField] private float scatterRadius = 0.3f;  // 30cm radius for scattering organs
    [SerializeField] private float scatterHeightOffset = 0.2f;  // Height offset for scattered organs

    [Header("Torso Dimensions (in meters)")]
    [SerializeField] private Vector3 torsoDimensions = new Vector3(0.3f, 0.6f, 0.25f);  // 30cm x 60cm x 25cm

    private Transform torsoTransform;
    private bool organsScattered = false;

    private void Awake()
    {
        InitializeOrganData();
    }

    /// <summary>
    /// Initialize organ data with their correct anatomical positions
    /// Positions are relative to the torso center
    /// </summary>
    private void InitializeOrganData()
    {
        organs.Clear();

        // Define initial positions for each organ (relative to torso center)
        // Y-axis: vertical (up/down), X-axis: left/right, Z-axis: front/back

        // Lungs (left and right, upper chest)
        organs.Add(new OrganData("Left Lung", OrganType.Lung)
        {
            initialPosition = new Vector3(-0.08f, 0.15f, 0f),
            initialRotation = Quaternion.identity
        });

        organs.Add(new OrganData("Right Lung", OrganType.Lung)
        {
            initialPosition = new Vector3(0.08f, 0.15f, 0f),
            initialRotation = Quaternion.identity
        });

        // Heart (center-left, upper chest)
        organs.Add(new OrganData("Heart", OrganType.Heart)
        {
            initialPosition = new Vector3(-0.02f, 0.12f, 0.02f),
            initialRotation = Quaternion.identity
        });

        // Liver (right side, middle abdomen)
        organs.Add(new OrganData("Liver", OrganType.Liver)
        {
            initialPosition = new Vector3(0.06f, 0.02f, 0f),
            initialRotation = Quaternion.identity
        });

        // Stomach (left side, upper abdomen)
        organs.Add(new OrganData("Stomach", OrganType.Stomach)
        {
            initialPosition = new Vector3(-0.05f, 0.05f, 0.02f),
            initialRotation = Quaternion.identity
        });

        // Pancreas (center, behind stomach)
        organs.Add(new OrganData("Pancreas", OrganType.Pancreas)
        {
            initialPosition = new Vector3(0f, 0.03f, -0.03f),
            initialRotation = Quaternion.identity
        });

        // Spleen (left side, behind stomach)
        organs.Add(new OrganData("Spleen", OrganType.Spleen)
        {
            initialPosition = new Vector3(-0.08f, 0.04f, -0.02f),
            initialRotation = Quaternion.identity
        });

        // Small Intestine (center, lower abdomen)
        organs.Add(new OrganData("Small Intestine", OrganType.SmallIntestine)
        {
            initialPosition = new Vector3(0f, -0.08f, 0f),
            initialRotation = Quaternion.identity
        });

        // Large Intestine (surrounding small intestine)
        organs.Add(new OrganData("Large Intestine", OrganType.LargeIntestine)
        {
            initialPosition = new Vector3(0f, -0.10f, 0f),
            initialRotation = Quaternion.identity
        });

        // Bladder (center, lowest point)
        organs.Add(new OrganData("Bladder", OrganType.Bladder)
        {
            initialPosition = new Vector3(0f, -0.22f, 0f),
            initialRotation = Quaternion.identity
        });
    }

    /// <summary>
    /// Set the torso transform reference
    /// </summary>
    public void SetTorsoTransform(Transform torso)
    {
        torsoTransform = torso;
    }

    /// <summary>
    /// Create organ game objects and place them at initial positions
    /// </summary>
    public void CreateOrgans()
    {
        if (torsoTransform == null)
        {
            Debug.LogError("Torso transform not set!");
            return;
        }

        foreach (var organ in organs)
        {
            // Create a simple organ representation (cube for now)
            GameObject organObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            organObj.name = organ.organName;
            organObj.transform.parent = torsoTransform;
            organObj.transform.localPosition = organ.initialPosition;
            organObj.transform.localRotation = organ.initialRotation;
            organObj.transform.localScale = Vector3.one * 0.04f;  // 4cm diameter

            // Add components for interaction
            organObj.AddComponent<OrganDragHandler>().Initialize(organ, this);

            // Set color based on organ type
            Renderer renderer = organObj.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(Shader.Find("Standard"));
                mat.color = GetOrganColor(organ.organType);
                renderer.material = mat;
            }

            organ.organModel = organObj;
            organ.currentPosition = organ.initialPosition;
        }
    }

    /// <summary>
    /// Scatter organs to random positions around the torso
    /// </summary>
    public void ScatterOrgans()
    {
        if (torsoTransform == null || organsScattered)
            return;

        foreach (var organ in organs)
        {
            if (organ.organModel != null)
            {
                // Generate random position around the torso
                Vector3 randomOffset = new Vector3(
                    Random.Range(-scatterRadius, scatterRadius),
                    Random.Range(-scatterRadius, scatterRadius) + scatterHeightOffset,
                    Random.Range(-scatterRadius, scatterRadius)
                );

                organ.organModel.transform.localPosition = organ.initialPosition + randomOffset;
                organ.currentPosition = organ.organModel.transform.localPosition;
                organ.isPlacedCorrectly = false;

                // Add slight random rotation
                organ.organModel.transform.localRotation = Random.rotation;
            }
        }

        organsScattered = true;
        Debug.Log("Organs scattered to random positions");
    }

    /// <summary>
    /// Check if an organ is placed correctly at its initial position
    /// </summary>
    public bool CheckOrganPlacement(OrganData organ)
    {
        if (organ.organModel == null)
            return false;

        float distance = Vector3.Distance(organ.organModel.transform.localPosition, organ.initialPosition);
        organ.isPlacedCorrectly = distance <= organ.placementThreshold;

        if (organ.isPlacedCorrectly)
        {
            // Snap to correct position
            organ.organModel.transform.localPosition = organ.initialPosition;
            organ.organModel.transform.localRotation = organ.initialRotation;

            // Visual feedback - change color to indicate correct placement
            Renderer renderer = organ.organModel.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.green;
            }

            Debug.Log($"{organ.organName} placed correctly!");
        }

        return organ.isPlacedCorrectly;
    }

    /// <summary>
    /// Check if all organs are placed correctly
    /// </summary>
    public bool AreAllOrgansPlaced()
    {
        foreach (var organ in organs)
        {
            if (!organ.isPlacedCorrectly)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Get color for organ based on type
    /// </summary>
    private Color GetOrganColor(OrganType type)
    {
        switch (type)
        {
            case OrganType.Lung: return new Color(1f, 0.8f, 0.8f);  // Light pink
            case OrganType.Heart: return new Color(0.8f, 0.2f, 0.2f);  // Red
            case OrganType.Stomach: return new Color(1f, 0.9f, 0.8f);  // Pale
            case OrganType.Liver: return new Color(0.6f, 0.3f, 0.2f);  // Dark red-brown
            case OrganType.Pancreas: return new Color(0.9f, 0.8f, 0.6f);  // Tan
            case OrganType.Spleen: return new Color(0.5f, 0.2f, 0.3f);  // Purple-red
            case OrganType.SmallIntestine: return new Color(1f, 0.9f, 0.9f);  // Light pink
            case OrganType.LargeIntestine: return new Color(0.9f, 0.8f, 0.8f);  // Pink
            case OrganType.Bladder: return new Color(0.9f, 0.9f, 0.6f);  // Yellow
            default: return Color.gray;
        }
    }

    /// <summary>
    /// Reset all organs to their initial positions
    /// </summary>
    public void ResetOrgans()
    {
        organsScattered = false;
        foreach (var organ in organs)
        {
            if (organ.organModel != null)
            {
                organ.organModel.transform.localPosition = organ.initialPosition;
                organ.organModel.transform.localRotation = organ.initialRotation;
                organ.currentPosition = organ.initialPosition;
                organ.isPlacedCorrectly = false;

                // Reset color
                Renderer renderer = organ.organModel.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = GetOrganColor(organ.organType);
                }
            }
        }
    }

    public List<OrganData> GetOrgans()
    {
        return organs;
    }

    public bool AreOrgansScattered()
    {
        return organsScattered;
    }
}
