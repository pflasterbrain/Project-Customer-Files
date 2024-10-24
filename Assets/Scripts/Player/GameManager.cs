using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int playerMoney = 100; // Initialize with starting money
    public TMP_Text moneyText; // Reference to the TMP_Text for displaying money
    public Vector3 savedPlayerPosition; // Store the player's position
    private string lastSceneName; // Store the name of the last scene

    private void Awake()
    {
        // Implementing the singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object between scene loads

            // Subscribe to the scene loaded and scene unloaded events
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Automatically assign the moneyText reference
        AssignMoneyText();

        // Load the player's position if the scene is "SampleScene"
        if (scene.name == "SampleScene 1")
        {
            RestorePlayerPosition();
        }

        UpdateMoneyDisplay(); // Update the money display when a scene is loaded
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // Save the player's position only when leaving "SampleScene"
        if (scene.name == "SampleScene 1")
        {
            SavePlayerPosition();
        }
    }

    // Method to find and assign the moneyText
    private void AssignMoneyText()
    {
        // Find the TMP_Text object in the current scene
        moneyText = GameObject.Find("MoneyText")?.GetComponent<TMP_Text>();

        if (moneyText == null)
        {
            Debug.LogWarning("MoneyText not found in the scene. Please make sure it exists.");
        }
    }

    // Method to save the player's position before loading a new scene
    private void SavePlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            savedPlayerPosition = player.transform.position;
            Debug.Log("Player position saved: " + savedPlayerPosition);
        }
    }

    // Method to restore the player's position after the new scene is loaded
    private void RestorePlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.transform.position = savedPlayerPosition;
            Debug.Log("Player position restored: " + savedPlayerPosition);
        }
        else
        {
            Debug.LogWarning("Player object not found in the scene.");
        }
    }

    // Method to add money
    public void AddMoney(int amount)
    {
        playerMoney += amount;
        UpdateMoneyDisplay(); // Update the UI display after adding money
    }

    // Method to subtract money
    public void SubtractMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            UpdateMoneyDisplay(); // Update the UI display after subtracting money
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    // Method to update the money display
    public void UpdateMoneyDisplay()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: $" + playerMoney.ToString("F2");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events when this instance is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}