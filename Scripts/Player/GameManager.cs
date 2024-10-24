using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int playerMoney = 500; // Initialize with starting money
    private int initialMoney = 500; // Store the initial money for reset purposes
    public TMP_Text moneyText; // Reference to the TMP_Text for displaying money
    public Vector3 savedPlayerPosition; // Store the player's position
    private string lastSceneName; // Store the name of the last scene
    public bool isGameResetting = false; // Flag to check if the game is being reset

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object between scene loads
            initialMoney = playerMoney; // Store initial money for resets
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to events
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            Debug.Log("GameManager instance created with initial money: " + initialMoney);
        }
        else
        {
            Debug.Log("Duplicate GameManager found. Destroying this instance: " + gameObject.name);
            Destroy(gameObject); // Destroy duplicate
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignMoneyText();

        // If the game is resetting, reset necessary data
        if (isGameResetting)
        {
            Debug.Log("Resetting GameManager on scene load.");
            ResetGameManager(); // Reset money, position, etc.
            UpdateMoneyDisplay(); // Ensure the display is updated after reset
            isGameResetting = false; // Reset the flag
        }

        lastSceneName = scene.name; // Update the last scene name

        // Restore player position in the specific scene
        if (scene.name == "SampleScene 1")
        {
            RestorePlayerPosition();
        }

        UpdateMoneyDisplay(); // Update the money display when a scene is loaded
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == "SampleScene 1")
        {
            SavePlayerPosition();
        }
    }

    // Method to fully reset the game
    public void FullGameReset()
    {
        Debug.Log("Resetting the entire game...");

        // Ensure inventory reset
        PlayerInventory.Instance.ResetInventory(); // Ensure inventory reset

        // Reset player's money to initial value
        playerMoney = initialMoney;
        Debug.Log("Player money reset to initial value: " + playerMoney);

        savedPlayerPosition = Vector3.zero; // Reset player position if necessary
        isGameResetting = true; // Set flag to trigger reset logic on next load

        // Reload the starting scene
        SceneManager.LoadScene("house_store"); // Replace with your starting scene name
    }

    // Method to reset the GameManager during a scene reload or full reset
    private void ResetGameManager()
    {
        playerMoney = initialMoney; // Reset player's money to initial value
        Debug.Log("GameManager has been reset. Current money: $" + playerMoney);
        savedPlayerPosition = Vector3.zero; // Optionally reset player position
    }

    private void AssignMoneyText()
    {
        moneyText = GameObject.Find("MoneyText")?.GetComponent<TMP_Text>();
        if (moneyText == null)
        {
            Debug.LogWarning("MoneyText not found in the scene.");
        }
    }

    private void SavePlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            savedPlayerPosition = player.transform.position;
            Debug.Log("Player position saved: " + savedPlayerPosition);
        }
    }

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

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        UpdateMoneyDisplay();
    }

    public void SubtractMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            UpdateMoneyDisplay();
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    public void UpdateMoneyDisplay()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: $" + playerMoney.ToString("F2");
            Debug.Log("Updated money display: " + moneyText.text);
        }
        else
        {
            Debug.LogWarning("MoneyText is null, cannot update display.");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
