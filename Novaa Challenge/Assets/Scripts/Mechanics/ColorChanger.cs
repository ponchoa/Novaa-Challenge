using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The colors that will be displayed in order with a fade")]
    Color[] colorsArray;

    [SerializeField]
    [Tooltip("The duration in seconds of the fade transition between each color")]
    float fadeDuration = 5f;

    [SerializeField]
    [Tooltip("Whether or not the background sprite is removed to allow for a single color")]
    bool overrideSprite = false;

    [SerializeField]
    [Tooltip("Whether or not the transition between colors will have a smoothing effet")]
    bool smoothTransition = false;

    [SerializeField]
    [Tooltip("The background image to affect")]
    Image image;

    Coroutine colorCoroutine;

    private void Start()
    {
        if (CheckForColors())
        {
            CheckImage();
            SetupImage();
            StartColorChange();
        }
    }

    void SetupImage()
    {
        if (overrideSprite)
        {
            image.sprite = null;
        }
    }

    #region Warnings
    bool CheckForColors()
    {
        if (colorsArray.Length <= 0)
        {
            Debug.LogWarning("ColorChanger (" + name + ") : The colors array is empty.");
            return false;
        }
        return true;
    }
    void CheckImage()
    {
        if (image is null)
        {
            Debug.LogWarning("ColorChanger (" + name + ") : The image wasn't properly set in the inspector.");
            image = GetComponentInChildren<Image>();
        }
    }
    #endregion

    #region Coroutine
    public void StartColorChange()
    {
        if (colorCoroutine is null && colorsArray.Length > 0)
        {
            colorCoroutine = StartCoroutine(ColorChangeCoroutine());
        }
    }
    public void StopColorChange()
    {
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
            colorCoroutine = null;
        }
    }

    IEnumerator ColorChangeCoroutine()
    {
        int currentIndex = 0;
        while (true)
        {
            float elapsedTime = 0f;
            Color startColor = colorsArray[currentIndex];
            Color endColor = colorsArray[(currentIndex + 1) % colorsArray.Length];
            while (elapsedTime <= fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float time = smoothTransition ? Mathf.SmoothStep(0f, 1f, elapsedTime / fadeDuration) : elapsedTime / fadeDuration;
                image.color = Color.Lerp(startColor, endColor, time);

                yield return null;
            }
            currentIndex++;
            currentIndex %= colorsArray.Length;
        }
    }
    #endregion
}
