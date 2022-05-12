using NovaaTest.Structs;
using UnityEngine;

namespace NovaaTest.SCObjects
{
    [CreateAssetMenu(fileName = "Question", menuName = "Quiz/Question", order = 2)]
    public class QuestionScriptableObject : ScriptableObject
    {
        [TextArea(1, 3)]
        [Tooltip("The question text that will be displayed at the top of the screen")]
        public string questionStatement;

        [Tooltip("All possible answers")]
        public AnswerStruct[] answerArray;

        /// <summary>
        /// Used to check if the question was correctly set up in the inspector. The question should be skipped if this is false.
        /// </summary>
        [HideInInspector]
        public bool isValid = true;

        public int CorrectAnswerIndex
        {
            get
            {
                for (int i = 1; i < answerArray.Length; i++)
                {
                    if (answerArray[i].isCorrect)
                    {
                        return i;
                    }
                }
                return 0;
            }
        }
    }
}