using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main game manager that controls the game flow, timer, and win conditions
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    private bool isARReady = false;
    private bool isGameStarted = false;
    private bool isGameComplete = false;

    [Header("Timer")]
    private float gameTime = 0f;
    private bool timerRunning = false;

    [Header("References")]
    [SerializeField] private ARImageTracker arImageTracker;
    [SerializeField] private OrganManager organManager;
    [SerializeField] private TransparentTorsoController torsoController;

    [Header("UI References")]
    [SerializeField] private Text statusText;
    [SerializeField] private Text timerText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Text instructionText;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        InitializeGame();
    }

    private void Update()
    {
        // Update timer if game is running
        if (timerRunning && isGameStarted && !isGameComplete)
        {
            gameTime += Time.deltaTime;
            UpdateTimerUI();
        }

        // Update status text
        UpdateStatusUI();
    }

    /// <summary>
    /// Initialize game components
    /// </summary>
    private void InitializeGame()
    {
        // Find references if not assigned
        if (arImageTracker == null)
            arImageTracker = FindObjectOfType<ARImageTracker>();

        if (organManager == null)
            organManager = FindObjectOfType<OrganManager>();

        if (torsoController == null)
            torsoController = FindObjectOfType<TransparentTorsoController>();

        // Setup UI
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
            startButton.interactable = false;  // Disabled until AR is ready
        }

        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetGame);
            resetButton.gameObject.SetActive(false);
        }

        UpdateInstructionText("Scan the reference image to begin...");
    }

    /// <summary>
    /// Called when AR image is successfully tracked
    /// </summary>
    public void OnARImageTracked()
    {
        if (isARReady)
            return;

        isARReady = true;
        Debug.Log("AR image tracked successfully!");

        // Create organs
        GameObject spawnedTorso = arImageTracker.GetSpawnedTorso();
        if (spawnedTorso != null && organManager != null)
        {
            // Find or create torso controller
            torsoController = spawnedTorso.GetComponentInChildren<TransparentTorsoController>();
            if (torsoController == null)
            {
                GameObject torsoObj = new GameObject("TransparentTorso");
                torsoObj.transform.parent = spawnedTorso.transform;
                torsoObj.transform.localPosition = Vector3.zero;
                torsoObj.transform.localRotation = Quaternion.identity;
                torsoController = torsoObj.AddComponent<TransparentTorsoController>();
            }

            // Set torso reference in organ manager
            organManager.SetTorsoTransform(torsoController.transform);

            // Create all organs at their initial positions
            organManager.CreateOrgans();
        }

        // Enable start button
        if (startButton != null)
        {
            startButton.interactable = true;
        }

        UpdateInstructionText("AR ready! Press 'Start Game' to begin.");
    }

    /// <summary>
    /// Start the game - scatter organs and start timer
    /// </summary>
    public void StartGame()
    {
        if (!isARReady || isGameStarted)
            return;

        isGameStarted = true;
        isGameComplete = false;
        gameTime = 0f;
        timerRunning = true;

        // Scatter organs to random positions
        if (organManager != null)
        {
            organManager.ScatterOrgans();
        }

        // Update UI
        if (startButton != null)
        {
            startButton.gameObject.SetActive(false);
        }

        if (resetButton != null)
        {
            resetButton.gameObject.SetActive(true);
        }

        UpdateInstructionText("Drag the organs back to their correct positions!");

        Debug.Log("Game started!");
    }

    /// <summary>
    /// Called when all organs are placed correctly
    /// </summary>
    public void OnGameComplete()
    {
        if (isGameComplete)
            return;

        isGameComplete = true;
        timerRunning = false;

        // Calculate final time
        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime % 60f);
        int milliseconds = Mathf.FloorToInt((gameTime * 100f) % 100f);

        string completionMessage = $"Congratulations! You completed the puzzle!\nTime: {minutes:00}:{seconds:00}:{milliseconds:00}";
        UpdateInstructionText(completionMessage);

        Debug.Log(completionMessage);

        // Show reset button
        if (resetButton != null)
        {
            resetButton.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Reset the game to initial state
    /// </summary>
    public void ResetGame()
    {
        isGameStarted = false;
        isGameComplete = false;
        gameTime = 0f;
        timerRunning = false;

        // Reset organs
        if (organManager != null)
        {
            organManager.ResetOrgans();
        }

        // Update UI
        if (startButton != null)
        {
            startButton.gameObject.SetActive(true);
            startButton.interactable = isARReady;
        }

        if (resetButton != null)
        {
            resetButton.gameObject.SetActive(false);
        }

        UpdateInstructionText(isARReady ? "Press 'Start Game' to begin." : "Scan the reference image to begin...");

        Debug.Log("Game reset!");
    }

    /// <summary>
    /// Update status text UI
    /// </summary>
    private void UpdateStatusUI()
    {
        if (statusText == null)
            return;

        if (!isARReady)
        {
            statusText.text = "Status: Scanning for image...";
        }
        else if (!isGameStarted)
        {
            statusText.text = "Status: Ready to start";
        }
        else if (isGameComplete)
        {
            statusText.text = "Status: Completed!";
        }
        else
        {
            statusText.text = "Status: Playing";
        }
    }

    /// <summary>
    /// Update timer text UI
    /// </summary>
    private void UpdateTimerUI()
    {
        if (timerText == null)
            return;

        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime % 60f);
        int milliseconds = Mathf.FloorToInt((gameTime * 100f) % 100f);

        timerText.text = $"Time: {minutes:00}:{seconds:00}:{milliseconds:00}";
    }

    /// <summary>
    /// Update instruction text
    /// </summary>
    private void UpdateInstructionText(string message)
    {
        if (instructionText != null)
        {
            instructionText.text = message;
        }
    }

    /// <summary>
    /// Check if game has started
    /// </summary>
    public bool IsGameStarted()
    {
        return isGameStarted && !isGameComplete;
    }

    /// <summary>
    /// Check if AR is ready
    /// </summary>
    public bool IsARReady()
    {
        return isARReady;
    }

    /// <summary>
    /// Get current game time
    /// </summary>
    public float GetGameTime()
    {
        return gameTime;
    }
}
