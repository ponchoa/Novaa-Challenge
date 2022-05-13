using UnityEngine;
using UnityEngine.UI;

namespace NovaaTest.Mechanics
{
    [RequireComponent(typeof(Animator), typeof(Image))]
    public class AnswerButtonAnimation : MonoBehaviour
    {
        /// <summary>
        /// A reference to the correct answer. Both used for checking the answer and calling the animation on it.
        /// </summary>
        public AnswerButtonAnimation correctAnswer { get; set; }

        Animator animator;
        Image image;
        Color startColor;

        private void Awake()
        {
            SetStartingValues();
        }

        void SetStartingValues()
        {
            animator = GetComponent<Animator>();
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
        public void OnButtonClick()
        {
            if (correctAnswer is null)
                return;
            if (correctAnswer == this)
            {
                StartCorrectAnimation();
            }
            else
            {
                StartWrongAnimation();
                correctAnswer.StartCorrectAnimation();
            }
        }

        void StartCorrectAnimation()
        {
            animator.SetBool("IsCorrect", true);
            animator.SetTrigger("OnClick");
        }

        void StartWrongAnimation()
        {
            animator.SetBool("IsCorrect", false);
            animator.SetTrigger("OnClick");
        }
    }
}