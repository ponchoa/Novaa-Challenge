using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsMenuController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("A reference to the text that will display the points")]
    TextMeshProUGUI amountText;
    [SerializeField]
    [Tooltip("A reference to the text that will display the ranking message")]
    TextMeshProUGUI rankingText;

    /// <summary>
    /// A simple string used to store the color of the results amount text.
    /// </summary>
    string colorText = "FF0000";

    private void Start()
    {
        if (!CurrentCategory.Instance.isAvailable)
        {
            Debug.LogWarning("ResultsMenuController (" + name + ") : The current category was set as unavailable. Did you load this scene at the correct time?");
        }

        DisplayResults();
    }

    /// <summary>
    /// Displays the results of the Current Category to the screen using the different UI elements.
    /// </summary>
    void DisplayResults()
    {
        int correctAnswers = CurrentCategory.Instance.correctAnswers;
        int totalAnswers = CurrentCategory.Instance.currentCategory.questionsArray.Length;
        float correctPercent = correctAnswers / (float)totalAnswers; //Percentage of correct answers

        //Different rankings with different colors and comments
        if (correctPercent < .5f)
        {
            rankingText.text = "You can do better !";
        }
        else if (correctPercent < .75f)
        {
            colorText = "FFFF00";
            rankingText.text = "You are smart !";
        }
        else
        {
            colorText = "00FF00";
            rankingText.text = "WOW !";
        }

        //Constructing the string that will display the amount of good answers
        amountText.text = "You got\n";
        amountText.text += "<color=#" + colorText + ">";
        amountText.text += correctAnswers.ToString() + " / ";
        amountText.text += totalAnswers.ToString();
    }

    /// <summary>
    /// If the player clicks on the Back button.
    /// </summary>
    public void OnClickBackButton()
    {
        if (SceneLoaderController.Instance.LoadScene(SceneType.CATEGORIES))
        {
            CurrentCategory.Instance.Clear(); //We clear the current category container to prepare for the next set of questions.
            SceneLoaderController.Instance.UnloadScene(SceneType.RESULTS);
        }
    }

    /// <summary>
    /// If the player clicks on the Share button.
    /// </summary>
    public void OnClickShareButton()
    {
        //TODO: Share activity
    }
}
