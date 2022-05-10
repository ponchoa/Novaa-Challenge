using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizMenuController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The text (TMP) that will display the question statement")]
    TextMeshProUGUI statementText;

    [SerializeField]
    [Tooltip("All of the buttons in order that correspond to the answers")]
    Button[] answerButtonsArray;

    /// <summary>
    /// Used to prevent the user from clicking during animations
    /// </summary>
    [SerializeField]
    [Tooltip("The object that will block the raycast when enabled")]
    Transform raycastBlocker;

    [SerializeField]
    [Tooltip("The duration of the button feedback animation in seconds (set to zero to skip animations)")]
    float buttonAnimDuration = .4f;
    [SerializeField]
    [Tooltip("The wait time in seconds after the animation is done before loading the next question or the results screen")]
    float waitBetweenQuestions = 1.6f;

    /// <summary>
    /// The current question to display.
    /// </summary>
    QuestionScriptableObject question;
    /// <summary>
    /// The index of the next question in the category array.
    /// </summary>
    int questionIndex = 0;
    /// <summary>
    /// The animations of each button in the same order, cached to avoid unnessecary GetComponent calls
    /// </summary>
    AnswerButtonAnimation[] buttonsAnimControllers;

    private void Start()
    {
        CheckCategoryState();
        if (CheckButtonsReference())
        {
            FillButtonsAnimsControllersArray();
        }

        raycastBlocker.gameObject.SetActive(false);
        LoadNextQuestion();
    }

    /// <summary>
    /// We fill the buttonsAnimControllers array with the component of each button in the same order
    /// </summary>
    void FillButtonsAnimsControllersArray()
    {
        buttonsAnimControllers = new AnswerButtonAnimation[answerButtonsArray.Length];
        for (int i = 0; i < answerButtonsArray.Length; i++)
        {
            buttonsAnimControllers[i] = answerButtonsArray[i].GetComponent<AnswerButtonAnimation>();
            if (buttonsAnimControllers[i] is null)
            {
                Debug.LogWarning("QuizMenuController (" + name + ") : No AnswerButtonAnimation component attached to " + answerButtonsArray[i].name + ".");
            }
        }
    }

    /// <summary>
    /// Load the next question in the current category array.
    /// </summary>
    void LoadNextQuestion()
    {
        if (CurrentCategory.Instance.currentCategory.questionsArray.Length <= questionIndex)
        {
            //If the last question was answered, we load the result screen.
            LoadResultsScreen();
        }
        else
        {
            question = CurrentCategory.Instance.currentCategory.questionsArray[questionIndex];
            questionIndex++;

            DisplayQuestion();
        }
    }
    IEnumerator WaitBeforeNextQuestion()
    {
        raycastBlocker.gameObject.SetActive(true);
        yield return new WaitForSeconds(waitBetweenQuestions);
        raycastBlocker.gameObject.SetActive(false);
        LoadNextQuestion();
    }

    #region Warnings
    void CheckCategoryState()
    {
        if (!CurrentCategory.Instance.isAvailable)
        {
            Debug.LogWarning("QuizMenuController (" + name + ") : The current category was set as unavailable. Did you load this scene at the correct time?");
        }
    }
    bool CheckButtonsReference()
    {
        if (answerButtonsArray.Length <= 0)
        {
            Debug.LogWarning("QuizMenuController (" + name + ") : No reference to the answer buttons.");
            return false;
        }
        return true;
    }
    #endregion

    #region Display
    /// <summary>
    /// Unloads the quiz screen and loads the results.
    /// </summary>
    void LoadResultsScreen()
    {
        if (SceneLoaderController.Instance.LoadScene(SceneType.RESULTS))
        {
            SceneLoaderController.Instance.UnloadScene(SceneType.QUIZ);
        }
    }

    /// <summary>
    /// Displays the question for the user using the different UI elements.
    /// </summary>
    void DisplayQuestion()
    {
        statementText.text = question.questionStatement;
        ResetButtons();
        for (int i = 0; i < GetAmountOfAnswers(); i++)
        {
            SetupButtonAnimation(i);
            SetupAnswerButton(i);
        }
    }

    /// <summary>
    /// The amount of answers depending on the number of buttons and prepared answers to the current question. (max 5)
    /// </summary>
    /// <returns>The proper amount of possible answers</returns>
    int GetAmountOfAnswers()
    {
        return Mathf.Min(5, Mathf.Min(answerButtonsArray.Length, question.answersArray.Length));
    }

    #region Buttons Setup
    /// <summary>
    /// Removes Listeners to each button and disables it
    /// </summary>
    void ResetButtons()
    {
        foreach (Button button in answerButtonsArray)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Used to set up the animation of the button at a specific index
    /// </summary>
    /// <param name="currentIndex">The index of the button to set up</param>
    void SetupButtonAnimation(int currentIndex)
    {
        if (buttonsAnimControllers[currentIndex] != null && buttonAnimDuration > 0f)
        {
            buttonsAnimControllers[currentIndex].ResetColor();
            if (0 <= question.correctAnswerIndex && question.correctAnswerIndex < buttonsAnimControllers.Length)
            {
                AnswerButtonAnimation buttonAnim = buttonsAnimControllers[currentIndex]; //We need to cache it so that the listener will work.
                //We add the listener
                answerButtonsArray[currentIndex].onClick.AddListener(() => { buttonAnim.OnButtonClick(buttonAnimDuration); });
                //We specify the correct answer to each button (See: the summary of correctAnswer)
                buttonsAnimControllers[currentIndex].correctAnswer = buttonsAnimControllers[question.correctAnswerIndex];
            }
        }
    }

    /// <summary>
    /// Used to set up the button at a specific index
    /// </summary>
    /// <param name="currentIndex">The index of the button to set up</param>
    void SetupAnswerButton(int currentIndex)
    {
        answerButtonsArray[currentIndex].gameObject.SetActive(true);
        answerButtonsArray[currentIndex].GetComponent<UIButton>().ButtonText = question.answersArray[currentIndex];
        //We set the correct listeners depending on if the answer is the correct one.
        if (currentIndex == question.correctAnswerIndex)
            answerButtonsArray[currentIndex].onClick.AddListener(OnCorrectAnswerClick);
        else
            answerButtonsArray[currentIndex].onClick.AddListener(OnWrongAnswerClick);
    }
    #endregion
    #endregion

    #region Buttons Listeners
    /// <summary>
    /// If the user chose the right answer.
    /// </summary>
    void OnCorrectAnswerClick()
    {
        CurrentCategory.Instance.correctAnswers++;
        StartCoroutine(WaitBeforeNextQuestion());
    }
    /// <summary>
    /// If the user chose the wrong answer.
    /// </summary>
    void OnWrongAnswerClick()
    {
        StartCoroutine(WaitBeforeNextQuestion());
    }
    #endregion
}
