using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Quiz/Question", order = 2)]
public class QuestionScriptableObject : ScriptableObject
{
    [Tooltip("The question text that will be displayed at the top of the screen")]
    public string questionStatement;

    [Tooltip("The element of the array that corresponds to the correct answer")]
    public int correctAnswerIndex;
    [Tooltip("All possible answers")]
    public string[] answersArray;
}
