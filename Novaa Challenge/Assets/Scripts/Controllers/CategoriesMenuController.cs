using NovaaTest.Enums;
using NovaaTest.Mechanics;
using NovaaTest.SCObjects;
using UnityEngine;
using UnityEngine.UI;

namespace NovaaTest.Controllers
{
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
            if (!CheckCategoriesReferences())
                return;
            if (!CheckButtonPrefab())
                return;
            if (!CheckVerticalLayoutGroupReference())
                return;

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
                if (buttonGO != null)
                {
                    UIButton button = buttonGO.GetComponent<UIButton>();
                    if (button != null)
                    {
                        if (button.ButtonText != null)
                            button.ButtonText = categories[i].categoryName;
                        // We have to cache the category for the listener.
                        CategoryScriptableObject category = categories[i];
                        if (category != null)
                            buttonGO.GetComponent<Button>()?.onClick.AddListener(() => { OnCategoryButtonClick(category); });
                    }
                }
            }
        }

        #region Checks
        /// <summary>
        /// Checks if there are categories set up in the inspector.
        /// </summary>
        /// <returns>Whether or not the categories were properly referenced.</returns>
        bool CheckCategoriesReferences()
        {
            if (categories.Length <= 0)
            {
                Debug.LogError($"CategoriesMenuController({name}) : No categories were listed in the object", this);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Checks if there is a prefab set for the categories buttons.
        /// </summary>
        /// <returns>Whether or not the prefab was properly referenced.</returns>
        bool CheckButtonPrefab()
        {
            if (categoryButtonPrefab is null)
            {
                Debug.LogError($"CategoriesMenuController({name}) : No button prefab was specified for the categories", this);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Checks if there is a vertical layout group referenced in the inspector.
        /// </summary>
        /// <returns>Whether or not the vertical layout group was properly referenced.</returns>
        bool CheckVerticalLayoutGroupReference()
        {
            if (verticalLayoutGroup is null)
            {
                Debug.LogError($"CategoriesMenuController({name}) : No vertical layout group was specified for the buttons", this);
                return false;
            }
            return true;
        }
        #endregion

        #region Listener
        /// <summary>
        /// The listener that should be set on each button to set up the category container and load the next screen
        /// </summary>
        /// <param name="category">The category that will be saved to correctly set up the quiz</param>
        void OnCategoryButtonClick(CategoryScriptableObject category)
        {
            SetCurrentCategory(category);
            LoadNextScene();
        }

        /// <summary>
        /// Prepares the category for later access.
        /// </summary>
        /// <param name="category">The category that will be used.</param>
        void SetCurrentCategory(CategoryScriptableObject category)
        {
            CurrentCategory.Instance.currentCategory = category;
            CurrentCategory.Instance.isAvailable = true;
        }

        /// <summary>
        /// Loads the Quiz scene.
        /// </summary>
        void LoadNextScene()
        {
            GameStateController.Instance?.LoadNextState();
        }
        #endregion
    }
}
