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
        if (categories.Length <= 0) //If there are no categories set
        {
            Debug.LogWarning("CategoriesMenuController(" + name + ") : No categories were listed in the object");
            return;
        }
        if (categoryButtonPrefab is null) //If there is no prefab to instantiate for the category buttons
        {
            Debug.LogWarning("CategoriesMenuController(" + name + ") : No button prefab was specified for the categories");
            return;
        }
        if (verticalLayoutGroup is null) //If we forgot to reference the Vertical Layout Group
        {
            Debug.LogWarning("CategoriesMenuController(" + name + ") : No vertical layout group was specified for the buttons");
            return;
        }

        for (int i = 0; i < categories.Length; i++) //We place one button for each category in the Layout Group
        {
            GameObject buttonGO = Instantiate(categoryButtonPrefab, verticalLayoutGroup);
            UIButton button = buttonGO.GetComponent<UIButton>();
            button.ButtonText = categories[i].categoryName;
            //When a button is clicked, we set the game to the corresponding category
            buttonGO.GetComponent<Button>().onClick.AddListener(() => { SetCurrentCategory(button.ButtonText); });
        }
    }

    /// <summary>
    /// The listener that should be set on each button to set up the category container and load the next screen.
    /// </summary>
    /// <param name="categoryName">The name of the category that will be saved to correctly set up the quiz.</param>
    public void SetCurrentCategory(string categoryName)
    {
        for (int i = 0; i < categories.Length; i++)
        {
            if (categoryName == categories[i].categoryName)
            {
                //We set the CurrentCategory container
                CurrentCategory.Instance.currentCategory = categories[i];
                CurrentCategory.Instance.isAvailable = true;
                break;
            }
        }


        //Then we load the next screen
        if (SceneLoaderController.Instance.LoadScene(SceneType.QUIZ))
        {
            SceneLoaderController.Instance.UnloadScene(SceneType.CATEGORIES);
        }
    }
}
