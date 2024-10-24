using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // Singleton instance
    public Image fadeImage; // Drag the UI Image here in the Inspector
    public GameObject loadingScreen; // Reference to the loading screen UI
    public GameObject sceneLoadedImage; // The image to be displayed after the scene loads
    public float fadeDuration = 1f;
    private bool isTransitioning = false; // Flag to prevent multiple transitions
    private bool imageDisplayed = false; // Flag to track if the image is displayed

    private Button buyButton;
    private Button backButton;

    // Define teleport position
    public Vector3 teleportPosition; // Set this in the inspector
    private bool shouldTeleport = false; // Flag to indicate teleportation should occur

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Preserve this object across scenes
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
            return;
        }

        SetImageAlpha(1f);
        StartCoroutine(FadeFromBlack());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignFadeImage();

        // Call FadeIn after the new scene is loaded and the fadeImage is reassigned
        StartCoroutine(FadeIn());

        // Display the sceneLoadedImage after the scene is loaded
        if (sceneLoadedImage != null)
        {
            sceneLoadedImage.SetActive(true); // Show the image when the scene loads
            Debug.Log("Scene loaded image is now active.");
            imageDisplayed = true; // Set flag to true
        }
        else
        {
            Debug.LogWarning("Scene loaded image not found.");
        }

        // Check if we should teleport the player
        if (shouldTeleport)
        {
            TeleportPlayer(); // Teleport the player if the flag is set
            shouldTeleport = false; // Reset the flag
        }

        if (scene.name == "Shop")
        {
            SetupShopButtons();
        }
    }

    private void AssignFadeImage()
    {
        fadeImage = GameObject.Find("fadeImage")?.GetComponent<Image>();
        sceneLoadedImage = GameObject.Find("SceneLoadedImage"); // Find the scene-loaded image

        if (fadeImage == null)
        {
            Debug.LogWarning("Fade Image not found in the scene. Please make sure it exists.");
        }

        if (sceneLoadedImage == null)
        {
            Debug.LogWarning("Scene Loaded Image not found. Please make sure it exists.");
        }
    }

    public void FadeAndLoadScene(string sceneName)
    {
        if (!isTransitioning)
        {
            StartCoroutine(FadeOutAndLoadScene(sceneName));
        }
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        isTransitioning = true;

        AssignFadeImage();

        if (fadeImage == null)
        {
            Debug.LogError("Fade Image not assigned. Make sure it exists in the current scene.");
            yield break;
        }

        yield return StartCoroutine(FadeOut());

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }

        yield return StartCoroutine(FadeIn());

        isTransitioning = false;
    }

    public IEnumerator FadeOut()
    {
        yield return StartCoroutine(Fade(0f, 1f)); // Fade from transparent to black
    }

    public IEnumerator FadeIn()
    {
        AssignFadeImage();
        while (fadeImage == null)
        {
            yield return null;
            AssignFadeImage();
        }
        yield return StartCoroutine(Fade(1f, 0f)); // Fade from black to transparent
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            color.a = alpha;
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }

    private void SetImageAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }
    }

    private IEnumerator FadeFromBlack()
    {
        yield return StartCoroutine(FadeIn());
    }

    public void StartGame()
    {
        FadeAndLoadScene("house_store");
    }

    public void OpenShop()
    {
        FadeAndLoadScene("Shop");
    }

    public void OpenOptions()
    {
        Debug.Log("Options menu opened.");
    }

    public void RestartCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        FadeAndLoadScene(currentSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }

    private void SetupShopButtons()
    {
        buyButton = GameObject.Find("buyButton")?.GetComponent<Button>();
        backButton = GameObject.Find("backButton")?.GetComponent<Button>();

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(BuyItem);
        }

        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(CloseShopAndTeleport); // Updated to use a new method
        }
    }

    private void BuyItem()
    {
        Debug.Log("Item bought! Implement your shop logic here.");
    }

    private void CloseShopAndTeleport()
    {
        shouldTeleport = true; // Set the flag to indicate teleportation should occur
        FadeAndLoadScene("house_store"); // Load the house_store scene
    }

    private void TeleportPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player"); // Assuming your player has a tag named "Player"
        if (player != null)
        {
            sceneLoadedImage.SetActive(false);
            player.transform.position = teleportPosition; // Teleport the player to the specified position
            Debug.Log("Player teleported to: " + teleportPosition);
        }
        else
        {
            Debug.LogWarning("Player object not found. Make sure the player is tagged correctly.");
        }
    }

    // This method detects if the user clicks and hides the image
    private void Update()
    {
        // Check if the image is displayed and if the screen is clicked
        if (imageDisplayed && Input.GetMouseButtonDown(0))
        {
            if (sceneLoadedImage != null)
            {
                sceneLoadedImage.SetActive(false); // Hide the image
            }
            imageDisplayed = false; // Reset the flag
        }
    }
}
