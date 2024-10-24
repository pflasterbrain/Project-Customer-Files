using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManagerScript : MonoBehaviour
{
    
    public GameObject loadingScreen; // Reference to the loading screen UI
    public UIManager sceneTransitionManager; // Reference to the UIManager for handling fades

    // Starts the main game scene
    public void StartGame()
    {
        sceneTransitionManager.FadeAndLoadScene("SampleScene 1");
    }

    // Opens the shop scene
    public void OpenShop()
    {
        sceneTransitionManager.FadeAndLoadScene("ShopScene");
    }

    // Opens the options menu
    public void OpenOptions()
    {
        Debug.Log("Options menu opened.");
    }

    // Restarts the current active scene
    public void RestartCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        sceneTransitionManager.FadeAndLoadScene(currentSceneName);
    }

    // Quits the game application
    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }

    // Loads a scene with a separate method for asynchronous scene loading
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithFade(sceneName));
    }

    // Coroutine to handle fading and loading asynchronously
    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        // Start fading out to black
        yield return StartCoroutine(sceneTransitionManager.FadeOut());

        // Optional: Show the loading screen if assigned
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // Start fading back in before loading the new scene
        yield return StartCoroutine(sceneTransitionManager.FadeIn());

        // Wait for an additional 1 second before loading the scene
        yield return new WaitForSeconds(1f);

        // Load the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            // Optional: Display loading progress here if needed
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            yield return null; // Wait until the next frame
        }

        // Once the scene is fully loaded, hide the loading screen
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }
    
}
