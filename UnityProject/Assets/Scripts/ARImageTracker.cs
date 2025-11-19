using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Tracks the reference image (10x10cm) and spawns the torso model when detected
/// </summary>
[RequireComponent(typeof(ARTrackedImageManager))]
public class ARImageTracker : MonoBehaviour
{
    [Header("AR Settings")]
    [SerializeField] private ARTrackedImageManager trackedImageManager;

    [Header("Game Objects")]
    [SerializeField] private GameObject torsoModelPrefab;

    private GameObject spawnedTorso;
    private bool isImageTracked = false;

    private void Awake()
    {
        if (trackedImageManager == null)
        {
            trackedImageManager = GetComponent<ARTrackedImageManager>();
        }
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Handle newly detected images
        foreach (var trackedImage in eventArgs.added)
        {
            SpawnTorsoModel(trackedImage);
        }

        // Update position of existing tracked images
        foreach (var trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking && spawnedTorso != null)
            {
                spawnedTorso.transform.position = trackedImage.transform.position;
                spawnedTorso.transform.rotation = trackedImage.transform.rotation;
                spawnedTorso.SetActive(true);
            }
            else if (trackedImage.trackingState == TrackingState.Limited && spawnedTorso != null)
            {
                // Optional: Keep the model visible or hide it
                // spawnedTorso.SetActive(false);
            }
        }

        // Handle removed images
        foreach (var trackedImage in eventArgs.removed)
        {
            if (spawnedTorso != null)
            {
                // Optional: Keep the model or destroy it
                // spawnedTorso.SetActive(false);
            }
        }
    }

    private void SpawnTorsoModel(ARTrackedImage trackedImage)
    {
        if (spawnedTorso == null && torsoModelPrefab != null)
        {
            // Spawn the torso model at the tracked image position
            spawnedTorso = Instantiate(torsoModelPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
            spawnedTorso.transform.parent = trackedImage.transform;

            // Scale the model to appropriate size
            // Reference image is 10x10cm, torso should be 60x30x25cm
            // Scale factor = torso height / reference image size = 60 / 10 = 6
            spawnedTorso.transform.localScale = Vector3.one;

            isImageTracked = true;

            Debug.Log($"Torso model spawned on tracked image: {trackedImage.referenceImage.name}");

            // Notify game manager that AR is ready
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.OnARImageTracked();
            }
        }
    }

    public bool IsImageTracked()
    {
        return isImageTracked;
    }

    public GameObject GetSpawnedTorso()
    {
        return spawnedTorso;
    }
}
