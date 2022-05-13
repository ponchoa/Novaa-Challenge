using NovaaTest.SCObjects;
using UnityEditor;

namespace NovaaTest.CustomInspector
{
    [CustomEditor(typeof(CategoryScriptableObject))]
    public class CategoryCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            CategoryScriptableObject category = (CategoryScriptableObject)target;
            if (category != null)
            {
                CheckCategoryName(category);
                CheckAllQuestionsValidity(category);
                CheckNumberOfQuestions(category);
            }
        }

        /// <summary>
        /// Checks if the category has a proper name.
        /// </summary>
        /// <param name="category">The CategoryScriptableObject to inspect.</param>
        void CheckCategoryName(CategoryScriptableObject category)
        {
            if (string.IsNullOrWhiteSpace(category.categoryName))
            {
                EditorGUILayout.HelpBox("The category doesn't have a proper name.", MessageType.Warning);
            }
        }
        /// <summary>
        /// Checks if the questions are all valid.
        /// </summary>
        /// <param name="category">The CategoryScriptableObject to inspect.</param>
        void CheckAllQuestionsValidity(CategoryScriptableObject category)
        {
            if (category.questionsArray is null) // It is null for a few frames when the category is created.
            {
                return;
            }
            for (int i = 0; i < category.questionsArray.Length; i++)
            {
                if (category.questionsArray[i] is null)
                {
                    EditorGUILayout.HelpBox($"The question at index {i} is invalid. Please check that you have correctly set it up, as it will be skipped.", MessageType.Warning);
                }
                else if (!category.questionsArray[i].isValid)
                {
                    EditorGUILayout.HelpBox($"The question {category.questionsArray[i].name} is invalid. Please check that you have correctly set it up, as it will be skipped.", MessageType.Warning);
                }
            }
        }
        /// <summary>
        /// Checks if there are between 2 and 5 valid questions.
        /// </summary>
        /// <param name="category">The CategoryScriptableObject to inspect.</param>
        void CheckNumberOfQuestions(CategoryScriptableObject category)
        {
            if (2 > category.NumberOfValidQuestions || category.NumberOfValidQuestions > 5)
            {
                EditorGUILayout.HelpBox("You must have between 2 and 5 valid questions in the category. It will function, but it isn't the correct amount.", MessageType.Warning);
            }
        }
    }
}