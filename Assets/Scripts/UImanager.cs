using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // Singleton instance
    public Image fadeImage; // Drag the UI Image here in the Inspector
    public GameObject loadingScreen; // Reference to the loading screen UI
    public float fadeDuration = 1f;
    private bool isTransitioning = false; // Flag to prevent multiple transitions

    // Button references for different scenes
    private Button buyButton;
    private Button backButton;

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

        // Set the initial color to be fully opaque (black screen)
        SetImageAlpha(1f);
        StartCoroutine(FadeFromBlack()); // Optionally start fading from black
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Automatically assign the fadeImage reference when the scene is loaded
        AssignFadeImage();

        // Check if the newly loaded scene is the shop scene and set up buttons
        if (scene.name == "Shop")
        {
            SetupShopButtons();
        }
    }

    private void AssignFadeImage()
    {
        fadeImage = GameObject.Find("fadeImage")?.GetComponent<Image>();

        if (fadeImage == null)
        {
            Debug.LogWarning("Fade Image not found in the scene. Please make sure it exists.");
        }
    }

    // Fades out and loads the specified scene
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

        // Reassign the fadeImage in case it was lost or not found
        AssignFadeImage();

        if (fadeImage == null)
        {
            Debug.LogError("Fade Image not assigned. Make sure it exists in the current scene.");
            yield break;
        }

        // Fade to black
        yield return StartCoroutine(FadeOut());

        // Load the new scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }

        // Reassign fadeImage again after the new scene is loaded
        AssignFadeImage();

        yield return StartCoroutine(FadeIn());

        isTransitioning = false;
    }


    public IEnumerator FadeOut()
    {
        yield return StartCoroutine(Fade(0f, 1f)); // Fade from transparent to black
    }

    public IEnumerator FadeIn()
    {
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

        // Ensure final alpha value is set
        color.a = endAlpha;
        fadeImage.color = color;
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }

    private IEnumerator FadeFromBlack()
    {
        yield return StartCoroutine(FadeIn());
    }

    // ----- Merged SceneTransitionManager Functions -----

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

    // ----- Shop Scene-Specific Setup -----

    private void SetupShopButtons()
    {
        // Locate the buttons in the shop scene using their GameObject names
        buyButton = GameObject.Find("buyButton")?.GetComponent<Button>();
        backButton = GameObject.Find("backButton")?.GetComponent<Button>();

        // Check if the buttons are found and set up their functionality
        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(BuyItem); // Assign BuyItem functionality
        }

        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() => FadeAndLoadScene("house_store")); // Go back to the main scene
        }
    }

    private void BuyItem()
    {
        Debug.Log("Item bought! Implement your shop logic here.");
    }
}
