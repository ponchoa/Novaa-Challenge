using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

[CustomEditor(typeof(CategoryScriptableObject))]
public class CategoryCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CategoryScriptableObject script = (CategoryScriptableObject)target;
        if (script != null)
        {
            CheckCategoryName(script);
            CheckAllQuestionsValidity(script);
            CheckNumberOfQuestions(script);
        }
    }

    void CheckCategoryName(CategoryScriptableObject category)
    {
        if (string.IsNullOrWhiteSpace(category.categoryName))
        {
            EditorGUILayout.HelpBox("The category doesn't have a proper name.", MessageType.Warning);
        }
    }
    void CheckAllQuestionsValidity(CategoryScriptableObject category)
    {
        if (category.questionsArray is null)
        {
            return;
        }
        for (int i = 0; i < category.questionsArray.Length; i++)
        {
            if (category.questionsArray[i] is null)
            {
                EditorGUILayout.HelpBox("The question at index " + i.ToString() + " is invalid. Please check that you have correctly set it up, as it will be skipped.", MessageType.Warning);
            }
            else if (!category.questionsArray[i].isValid)
            {
                EditorGUILayout.HelpBox("The question " + category.questionsArray[i].name + " is invalid. Please check that you have correctly set it up, as it will be skipped.", MessageType.Warning);
            }
        }
    }
    void CheckNumberOfQuestions(CategoryScriptableObject category)
    {
        if (2 > category.NumberOfValidQuestions || category.NumberOfValidQuestions > 5)
        {
            EditorGUILayout.HelpBox("You must have between 2 and 5 valid questions in the category. It will function, but it isn't the correct amount.", MessageType.Warning);
        }
    }
}
