using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoriesMenuController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The array of Category Scriptable Objects that are present in the game")]
    CategoryScriptableObject[] categories;
    [SerializeField]
    [Tooltip("The prefab of the button that will be instantiated fo each category in the menu")]
    GameObject categoryButtonPrefab;
    [SerializeField]
    [Tooltip("The \"Content\" object MUST be referenced here. It is the vertical layout group that will ontain the buttons")]
    Transform verticalLayoutGroup;

    private void Start()
    {
        CheckCategoriesReferences();
        CheckButtonPrefab();
        CheckVerticalLayoutGroupReference();

        FillCategoriesButtons();
    }

    /// <summary>
    /// Instantiate one button for each category in the vertical layout group
    /// </summary>
    void FillCategoriesButtons()
    {
        for (int i = 0; i < categories.Length; i++)
        {
            GameObject buttonGO = Instantiate(categoryButtonPrefab, verticalLayoutGroup);
            UIButton button = buttonGO.GetComponent<UIButton>();
            button.ButtonText = categories[i].categoryName;
            CategoryScriptableObject category = categories[i]; //We have to cache the category for the listener
            buttonGO.GetComponent<Button>().onClick.AddListener(() => { OnCategoryButtonClick(category); });
        }
    }

    #region Warnings
    void CheckCategoriesReferences()
    {
        if (categories.Length <= 0)
        {
            Debug.LogWarning("CategoriesMenuController(" + name + ") : No categories were listed in the object");
            return;
        }
    }
    void CheckButtonPrefab()
    {
        if (categoryButtonPrefab is null)
        {
            Debug.LogWarning("CategoriesMenuController(" + name + ") : No button prefab was specified for the categories");
            return;
        }
    }
    void CheckVerticalLayoutGroupReference()
    {
        if (verticalLayoutGroup is null)
        {
            Debug.LogWarning("CategoriesMenuController(" + name + ") : No vertical layout group was specified for the buttons");
            return;
        }
    }
    #endregion

    #region Listener
    /// <summary>
    /// The listener that should be set on each button to set up the category container and load the next screen
    /// </summary>
    /// <param name="category">The category that will be saved to correctly set up the quiz</param>
    public void OnCategoryButtonClick(CategoryScriptableObject category)
    {
        SetCurrentCategory(category);
        LoadNextScene();
    }

    public void SetCurrentCategory(CategoryScriptableObject category)
    {
        CurrentCategory.Instance.currentCategory = category;
        CurrentCategory.Instance.isAvailable = true;
    }

    void LoadNextScene()
    {
        if (SceneLoaderController.Instance.LoadScene(SceneType.QUIZ))
        {
            SceneLoaderController.Instance.UnloadScene(SceneType.CATEGORIES);
        }
    }
    #endregion
}
