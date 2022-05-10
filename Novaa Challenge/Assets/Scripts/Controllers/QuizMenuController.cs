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
    Button[] buttonAnswersArray;

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
    AnswerButtonAnimation[] buttonsAnims;

    private void Start()
    {
        if (!CurrentCategory.Instance.isAvailable)
        {
            Debug.LogWarning("QuizMenuController (" + name + ") : The current category was set as unavailable. Did you load this scene at the correct time?");
        }
        if (buttonAnswersArray.Length <= 0)
        {
            Debug.LogWarning("QuizMenuController (" + name + ") : No reference to the answer buttons.");
        }
        else
        {
            //We fill the buttonsAnim array with the component of each button in the same order.
            buttonsAnims = new AnswerButtonAnimation[buttonAnswersArray.Length];
            for (int i = 0; i < buttonAnswersArray.Length; i++)
            {
                buttonsAnims[i] = buttonAnswersArray[i].GetComponent<AnswerButtonAnimation>();
                if (buttonsAnims[i] is null)
                {
                    Debug.LogWarning("QuizMenuController (" + name + ") : No AnswerButtonAnimation component attached to " + buttonAnswersArray[i].name + ".");
                }
            }
        }

        //Right now the blocker isn't used, but it will be if there are some animations
        raycastBlocker.gameObject.SetActive(false);
        LoadNextQuestion();
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
            //We set the new question to display, increment the index and display the question.
            question = CurrentCategory.Instance.currentCategory.questionsArray[questionIndex];
            questionIndex++;

            DisplayQuestion();
        }
    }

    /// <summary>
    /// Displays the question for the user using the different UI elements.
    /// </summary>
    void DisplayQuestion()
    {
        statementText.text = question.questionStatement; //The statement
        foreach (Button button in buttonAnswersArray) //First we reset every button in the array
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
        for (int i = 0; i < Mathf.Min(5, Mathf.Min(buttonAnswersArray.Length, question.answersArray.Length)); i++) //Then we only set those needed, we also can't see more than 5 answers
        {
            //We set up the animation if it is possible and the duration is positive
            if (buttonsAnims[i] != null && buttonAnimDuration > 0f)
            {
                buttonsAnims[i].ResetColor();
                if (0 <= question.correctAnswerIndex && question.correctAnswerIndex < buttonsAnims.Length)
                {
                    AnswerButtonAnimation buttonAnim = buttonsAnims[i];
                    //We add the listener
                    buttonAnswersArray[i].onClick.AddListener(() => { buttonAnim.OnButtonClick(buttonAnimDuration); });
                    //We specify the correct answer to each button (See: the summary of correctAnswer)
                    buttonsAnims[i].correctAnswer = buttonsAnims[question.correctAnswerIndex];
                }
            }

            buttonAnswersArray[i].gameObject.SetActive(true);
            buttonAnswersArray[i].GetComponent<UIButton>().ButtonText = question.answersArray[i];
            //We set the correct listeners depending on if the answer is the correct one.
            if (i == question.correctAnswerIndex)
                buttonAnswersArray[i].onClick.AddListener(OnCorrectAnswerClick);
            else
                buttonAnswersArray[i].onClick.AddListener(OnWrongAnswerClick);
        }
    }

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

    IEnumerator WaitBeforeNextQuestion()
    {
        raycastBlocker.gameObject.SetActive(true);
        yield return new WaitForSeconds(waitBetweenQuestions);
        raycastBlocker.gameObject.SetActive(false);
        LoadNextQuestion();
    }
}
