using UnityEngine;

namespace NovaaTest.SCObjects
{
    [CreateAssetMenu(fileName = "Category", menuName = "Quiz/Category", order = 1)]
    public class CategoryScriptableObject : ScriptableObject
    {
        [Tooltip("The name of the category that will be displayed in the menu")]
        public string categoryName;

        [Tooltip("The array of Question scriptable objects corresponding to this category")]
        public QuestionScriptableObject[] questionsArray;

        public int NumberOfValidQuestions
        {
            get
            {
                if (questionsArray is null)
                    return 0;

                // Unfortunately we can't cache the value found, or it will never change due to Serialization.
                int res = questionsArray.Length;
                foreach (QuestionScriptableObject question in questionsArray)
                {
                    if (question is null || !question.isValid)
                        res--;
                }
                return res;
            }
        }
    }
}