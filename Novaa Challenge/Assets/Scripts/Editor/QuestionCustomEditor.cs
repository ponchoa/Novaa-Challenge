using NovaaTest.SCObjects;
using UnityEditor;

namespace NovaaTest.CustomInspector
{
    [CustomEditor(typeof(QuestionScriptableObject))]
    public class QuestionCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            QuestionScriptableObject question = (QuestionScriptableObject)target;
            if (question != null)
            {
                question.isValid = true;
                CheckNumberOfAnswers(question);
                CheckAnswersText(question);
                CheckIfAnyChecked(question);
            }
        }

        void CheckNumberOfAnswers(QuestionScriptableObject question)
        {
            if (question.answerArray is null) //It is null for a few frames when the question is created
                return;
            if (2 > question.answerArray.Length || question.answerArray.Length > 5)
            {
                EditorGUILayout.HelpBox("There must be between 2 and 5 possible answers", MessageType.Error);
                question.isValid = false;
            }
        }
        void CheckAnswersText(QuestionScriptableObject question)
        {
            if (question.answerArray is null) //It is null for a few frames when the question is created
                return;
            for (int i = 0; i < question.answerArray.Length; i++)
            {
                bool shouldBreak = false; //This flag avoids displaying the same warning multiple times in the inspector
                if (string.IsNullOrWhiteSpace(question.answerArray[i].text))
                {
                    EditorGUILayout.HelpBox("The answer at index " + i.ToString() + " is empty", MessageType.Error);
                    question.isValid = false;
                }
                else
                {
                    for (int j = i + 1; j < question.answerArray.Length; j++)
                    {
                        if (question.answerArray[i].text == question.answerArray[j].text)
                        {
                            shouldBreak = true;
                            EditorGUILayout.HelpBox("There are multiple answers with the same text", MessageType.Error);
                            question.isValid = false;
                            break;
                        }
                    }
                }
                if (shouldBreak)
                    break;
            }
        }
        void CheckIfAnyChecked(QuestionScriptableObject question)
        {
            if (question.answerArray is null)
                return;
            for (int i = 0; i < question.answerArray.Length; i++)
            {
                if (question.answerArray[i].isCorrect)
                    return;
            }
            EditorGUILayout.HelpBox("No answer has been checked as the correct one. By default the first answer will be used as the correct one.", MessageType.Warning);
        }
    }
}