using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [HideInInspector]
    public CategoryScriptableObject currentCategory;
    /// <summary>
    /// A universal boolean that governs if the current category is coherent with the game state
    /// </summary>
    [HideInInspector]
    public bool isAvailable;
    /// <summary>
    /// The amount of correct answers the player got during his play of this category
    /// </summary>
    [HideInInspector]
    public int correctAnswers;

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
