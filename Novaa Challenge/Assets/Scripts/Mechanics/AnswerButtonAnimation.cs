using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButtonAnimation : MonoBehaviour
{
    /// <summary>
    /// A reference to the correct answer. Both used for checking the answer and calling the animation on it.
    /// </summary>
    [HideInInspector]
    public AnswerButtonAnimation correctAnswer;

    [SerializeField]
    [Range(0f, 100f)]
    [Tooltip("The left/right offset of the shaking for the wrong answer animation.")]
    float xShakeOffset = 75f;

    /// <summary>
    /// We cache the coroutine, just in case we need to stop it, or check if it is running.
    /// </summary>
    Coroutine currentAnimationCoroutine;
    RectTransform rTransform;
    Image image;
    Color startColor;

    private void Awake()
    {
        SetupStartingValues();
    }

    /// <summary>
    /// Sets the necessary values, references to components, etc.
    /// </summary>
    void SetupStartingValues()
    {
        currentAnimationCoroutine = null;
        rTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        startColor = image.color;
    }

    /// <summary>
    /// Resets the button to its starting color
    /// </summary>
    public void ResetColor()
    {
        image.color = startColor;
    }

    /// <summary>
    /// Will call the corresponding animation whether the answer is correct or not. And also call the correct answer animation
    /// on the correct answer button.
    /// </summary>
    /// <param name="duration">The duration of the animation, to ensure everything is concurrent.</param>
    public void OnButtonClick(float duration)
    {
        if (correctAnswer == this)
        {
            StartCorrectAnswerAnimation(duration);
        }
        else
        {
            StartWrongAnswerAnimation(duration);
            correctAnswer.StartCorrectAnswerAnimation(duration);
        }
    }

    #region Animations
    /// <summary>
    /// Starts a coroutine that will animate a feedback, letting the user know that the answer was wrong.
    /// </summary>
    /// <param name="duration">The duration of the animation</param>
    void StartWrongAnswerAnimation(float duration)
    {
        if (currentAnimationCoroutine is null)
        {
            currentAnimationCoroutine = StartCoroutine(WrongAnswerCoroutine(duration));
        }
    }
    IEnumerator WrongAnswerCoroutine(float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPos = rTransform.localPosition; //The starting position of the button
        Vector3 rightPos = startPos + new Vector3(xShakeOffset, 0f, 0f); //The rightmost position of the shake
        Vector3 leftPos = startPos - new Vector3(xShakeOffset, 0f, 0f); //The leftmost position of the shake
        Color targetColor = Color.red;
        targetColor.a = startColor.a;

        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            float time = elapsedTime / duration;

            image.color = Color.Lerp(startColor, targetColor, Mathf.SmoothStep(0f, 1f, time));


            //TODO: Pretty up this mess
            if (time < .25f) //First quarter, we move to the right
            {
                time = Mathf.SmoothStep(0f, 1f, time) / .25f; //Time needs to be a value between 0 and 1 to serve as a percentage. We also smooth it.
                rTransform.localPosition = Vector3.Lerp(startPos, rightPos, time);
            }
            else if (time < .5f) //Second quarter, we move all the way to the left
            {
                time = (Mathf.SmoothStep(0f, 1f, time) - .25f) / .25f;
                rTransform.localPosition = Vector3.Lerp(rightPos, leftPos, time);
            }
            else if (time < .75f) //Third quarter, we move back to the right
            {
                time = (Mathf.SmoothStep(0f, 1f, time) - .5f) / .25f;
                rTransform.localPosition = Vector3.Lerp(leftPos, rightPos, time);
            }
            else //Last quarter, we move back to the start
            {
                time = (Mathf.SmoothStep(0f, 1f, time) - .75f) / .25f;
                rTransform.localPosition = Vector3.Lerp(rightPos, startPos, time);
            }

            yield return null;
        }

        currentAnimationCoroutine = null;
    }

    /// <summary>
    /// Starts a coroutine that will animate a feedback, letting the user know that the answer was right.
    /// </summary>
    /// <param name="duration">The duration of the animation</param>
    void StartCorrectAnswerAnimation(float duration)
    {
        if (currentAnimationCoroutine is null)
        {
            currentAnimationCoroutine = StartCoroutine(CorrectAnswerCoroutine(duration));
        }
    }
    IEnumerator CorrectAnswerCoroutine(float duration)
    {
        float elapsedTime = 0f;
        Color targetColor = Color.green;
        targetColor.a = startColor.a;

        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            image.color = Color.Lerp(startColor, targetColor, Mathf.SmoothStep(0f, 1f, elapsedTime / duration));
            yield return null;
        }

        currentAnimationCoroutine = null;
    }
    #endregion
}
