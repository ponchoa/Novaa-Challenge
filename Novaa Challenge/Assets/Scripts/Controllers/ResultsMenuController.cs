using UnityEngine;
using TMPro;
using NovaaTest.Mechanics;
using NovaaTest.Enums;

namespace NovaaTest.Controllers
{
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
        string colorText;

        private void Start()
        {
            if (!CheckCategory())
                return;
            CheckCategoryState();

            DisplayResults();
        }

        #region Checks
        /// <summary>
        /// Checks whether the current category is properly set.
        /// </summary>
        /// <returns>Whether the category is correctly set.</returns>
        bool CheckCategory()
        {
            if (CurrentCategory.Instance.currentCategory is null)
            {
                Debug.LogError($"ResultsMenuController ({name}) : The current category is null.", this);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Checks if the current category was set as available.
        /// </summary>
        void CheckCategoryState()
        {
            if (!CurrentCategory.Instance.isAvailable)
            {
                Debug.LogWarning($"ResultsMenuController ({name}) : The current category was set as unavailable. Did you load this scene at the correct time?", this);
            }
        }
        #endregion

        #region Display
        /// <summary>
        /// Displays the results of the Current Category to the screen using the different UI elements.
        /// </summary>
        void DisplayResults()
        {
            int correctAnswers = CurrentCategory.Instance.correctAnswers;
            int totalAnswers = CurrentCategory.Instance.currentCategory.NumberOfValidQuestions;

            SetRankingAndColor(correctAnswers / (float)totalAnswers);
            DisplayAmountText(correctAnswers, totalAnswers);
        }

        /// <summary>
        /// Sets the score color and ranking comment depending on the percentage of correct answers
        /// </summary>
        /// <param name="correctPercent">The percentage of correct answers over the total amount</param>
        void SetRankingAndColor(float correctPercent)
        {
            // Different rankings with different colors and comments.
            if (correctPercent < .5f)
            {
                colorText = "FF0000";
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
        }
        /// <summary>
        /// Constructing and setting the text that displays the score
        /// </summary>
        /// <param name="correctAnswers">The amount of correct answers the player got during his play</param>
        /// <param name="totalAnswers">The total possible amount of good answers for this category</param>
        void DisplayAmountText(int correctAnswers, int totalAnswers)
        {
            amountText.text = $"You got\n<color=#{colorText}>{correctAnswers} / {totalAnswers}";
        }
        #endregion

        #region Buttons Listeners
        /// <summary>
        /// If the player clicks on the Back button.
        /// </summary>
        public void OnClickBackButton()
        {
            CurrentCategory.Instance.Clear();
            GameStateController.Instance?.LoadNextState();
        }

        /// <summary>
        /// If the player clicks on the Share button.
        /// </summary>
        public void OnClickShareButton()
        {
            // TODO: Share activity
        }
        #endregion
    }
}