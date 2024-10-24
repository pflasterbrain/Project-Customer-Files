using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    /*
    public Image fadeImage; // Drag the UI Image here in the Inspector
    public float fadeDuration = 1f;

    private void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade Image is not assigned! Please assign it in the Inspector.");
            return;  // Exit if the image is not assigned
        }
        // Set the image to fully transparent at the start
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;
    }

    // Call this function to fade to black
    public void FadeToBlack()
    {
        StartCoroutine(Fade(0f, 1f)); // Fade from transparent to black
    }

    // Call this function to fade from black
    public void FadeFromBlack()
    {
        StartCoroutine(Fade(1f, 0f)); // Fade from black to transparent
    }

    // Fade coroutine
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
    }*/
}