using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Category", menuName = "Quiz/Category", order = 1)]
public class CategoryScriptableObject : ScriptableObject
{
    [Tooltip("The name of the category that will be displayed in the menu")]
    public string categoryName;

    [Tooltip("The array of Question scriptable objects corresponding to this category")]
    public QuestionScriptableObject[] questionsArray;
}
