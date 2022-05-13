using NovaaTest.SCObjects;
using UnityEngine;

namespace NovaaTest.Mechanics
{
    /// <summary>
    /// This is a Singleton class that stores the currently playing category for multiple references
    /// </summary>
    public class CurrentCategory : MonoBehaviour
    {
        #region Singleton
        private static CurrentCategory instance;
        public static CurrentCategory Instance
        {
            get
            {
                if (instance is null)
                    instance = FindObjectOfType<CurrentCategory>();
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// A universal reference to the current loaded category.
        /// </summary>
        public CategoryScriptableObject currentCategory { get; set; }
        /// <summary>
        /// A universal boolean that governs if the current category is coherent with the game state
        /// </summary>
        public bool isAvailable { get; set; }
        /// <summary>
        /// The amount of correct answers the player got during his play of this category
        /// </summary>
        public int correctAnswers { get; set; }

        private void Awake()
        {
            MakeInstance();
        }

        void MakeInstance()
        {
            if (instance is null)
                instance = this;
        }

        /// <summary>
        /// Used to reset the container to prepare for the next play.
        /// </summary>
        public void Clear()
        {
            correctAnswers = 0;
            currentCategory = null;
            isAvailable = false;
        }
    }
}