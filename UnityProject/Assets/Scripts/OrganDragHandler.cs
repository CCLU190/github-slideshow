using UnityEngine;

/// <summary>
/// Handles drag and drop interaction for individual organs
/// </summary>
[RequireComponent(typeof(Collider))]
public class OrganDragHandler : MonoBehaviour
{
    private OrganData organData;
    private OrganManager organManager;
    private Camera arCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private float dragDistance = 0.5f;  // Distance from camera while dragging

    private Vector3 originalScale;
    private Material originalMaterial;

    public void Initialize(OrganData data, OrganManager manager)
    {
        organData = data;
        organManager = manager;
        originalScale = transform.localScale;
    }

    private void Start()
    {
        arCamera = Camera.main;
        if (arCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }

        // Store original material
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.material;
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStarted())
            return;

        HandleInput();
    }

    private void HandleInput()
    {
        // Handle touch input for mobile AR
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    TryStartDrag(touch.position);
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        DragOrgan(touch.position);
                    }
                    break;

                case TouchPhase.Ended:
                    if (isDragging)
                    {
                        EndDrag();
                    }
                    break;
            }
        }
        // Handle mouse input for Unity Editor testing
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryStartDrag(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0) && isDragging)
            {
                DragOrgan(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0) && isDragging)
            {
                EndDrag();
            }
        }
    }

    private void TryStartDrag(Vector2 screenPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform)
            {
                isDragging = true;

                // Calculate offset between organ position and touch point
                Vector3 touchWorldPos = arCamera.ScreenToWorldPoint(
                    new Vector3(screenPosition.x, screenPosition.y, dragDistance)
                );
                offset = transform.position - touchWorldPos;

                // Visual feedback - scale up slightly
                transform.localScale = originalScale * 1.2f;

                Debug.Log($"Started dragging {organData.organName}");
            }
        }
    }

    private void DragOrgan(Vector2 screenPosition)
    {
        if (!isDragging)
            return;

        // Convert screen position to world position
        Vector3 newPosition = arCamera.ScreenToWorldPoint(
            new Vector3(screenPosition.x, screenPosition.y, dragDistance)
        );

        transform.position = newPosition + offset;
    }

    private void EndDrag()
    {
        if (!isDragging)
            return;

        isDragging = false;

        // Reset scale
        transform.localScale = originalScale;

        // Update current position in organ data
        organData.currentPosition = transform.localPosition;

        // Check if organ is placed correctly
        bool placedCorrectly = organManager.CheckOrganPlacement(organData);

        if (placedCorrectly)
        {
            // Play success sound/haptic feedback here
            Debug.Log($"{organData.organName} placed correctly!");

            // Check if all organs are placed
            if (organManager.AreAllOrgansPlaced())
            {
                GameManager.Instance.OnGameComplete();
            }
        }

        Debug.Log($"Ended dragging {organData.organName}");
    }

    private void OnMouseEnter()
    {
        // Visual feedback when hovering (for editor testing)
        if (!isDragging && GameManager.Instance.IsGameStarted())
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.yellow;
            }
        }
    }

    private void OnMouseExit()
    {
        // Reset visual feedback
        if (!isDragging)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && originalMaterial != null)
            {
                renderer.material.color = originalMaterial.color;
            }
        }
    }
}
