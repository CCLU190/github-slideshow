using UnityEngine;

/// <summary>
/// Data structure for organ information
/// </summary>
[System.Serializable]
public class OrganData
{
    [Header("Organ Information")]
    public string organName;
    public OrganType organType;
    public GameObject organModel;

    [Header("Position Settings")]
    public Vector3 initialPosition;  // Initial position within the torso
    public Quaternion initialRotation;
    public Vector3 currentPosition;

    [Header("Placement Settings")]
    public float placementThreshold = 0.05f;  // Distance threshold for correct placement (in meters, 5cm)
    public bool isPlacedCorrectly = false;

    public OrganData(string name, OrganType type)
    {
        organName = name;
        organType = type;
        isPlacedCorrectly = false;
    }
}

/// <summary>
/// Enum for different organ types
/// </summary>
public enum OrganType
{
    Lung,      // 肺
    Heart,     // 心
    Stomach,   // 胃
    Liver,     // 肝
    Pancreas,  // 胰
    Spleen,    // 脾
    SmallIntestine,  // 小腸
    LargeIntestine,  // 大腸
    Bladder    // 膀胱
}
