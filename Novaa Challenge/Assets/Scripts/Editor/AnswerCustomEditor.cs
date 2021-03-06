using UnityEngine;
using UnityEditor;
using NovaaTest.Structs;
using NovaaTest.SCObjects;

namespace NovaaTest.CustomInspector
{
    [CustomPropertyDrawer(typeof(AnswerStruct))]
    public class AnswerCustomEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // The rects that will determine the positions of the properties in AnswerStruct.
            var textRect = new Rect(position.x, position.y, position.xMax - 65, position.height);
            var checkRect = new Rect(position.xMax - 13, position.y, 20, position.height);

            // We draw the question text.
            EditorGUI.PropertyField(textRect, property.FindPropertyRelative("text"), GUIContent.none);

            // We begin to check for changes, so that if the user checks an answer as the correct one, it will disable the others.
            EditorGUI.BeginChangeCheck();
            // We draw the checkbox.
            EditorGUI.PropertyField(checkRect, property.FindPropertyRelative("isCorrect"), GUIContent.none);
            if (EditorGUI.EndChangeCheck()) // If there was any change.
            {
                UpdateCorrectAnswer(property);
            }

            EditorGUI.EndProperty();
        }

        /// <summary>
        /// When called, will set the isCorrect value of all Answers to false, except for the one we pas as parameter.
        /// </summary>
        /// <param name="property">The property that won't have its isCorrect value changed.</param>
        void UpdateCorrectAnswer(SerializedProperty property)
        {
            // We need to update the AnswerStruct early to be able to check which answer was previously checked and uncheck it.
            property.serializedObject.ApplyModifiedProperties();

            QuestionScriptableObject question = (QuestionScriptableObject)property.serializedObject.targetObject; //We get the question we are editing
            if (question != null)
            {
                for (int i = 0; i < question.answerArray.Length; i++)
                {
                    // To make sure we aren't unchecking the answer we just checked, we compare the text.
                    // That means that if two answers have the exact same text, we could check both, but if that is the case, the question will be marked as invalid.
                    if (question.answerArray[i].text != property.FindPropertyRelative("text").stringValue)
                    {
                        // We set all other answers to false.
                        question.answerArray[i].isCorrect = false;
                    }
                }
            }
        }
    }
}