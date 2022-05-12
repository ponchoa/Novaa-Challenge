using UnityEngine;
using TMPro;

namespace NovaaTest.Mechanics
{
    public class UIButton : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("This should be set to the text (TMP) of the button, nothing else")]
        TextMeshProUGUI buttonText;

        /// <summary>
        /// The Text that is displayed on the button
        /// </summary>
        public string ButtonText
        {
            get { return buttonText.text; }
            set { buttonText.text = value; }
        }

        private void Start()
        {
            CheckButtonTextReference();
        }

        #region Checks
        void CheckButtonTextReference()
        {
            if (buttonText is null)
            {
                buttonText = GetComponentInChildren<TextMeshProUGUI>();
                Debug.LogWarning("CategoryButton (" + name + ") : No text (TMP) reference was set in the inspector. The wrong text might have been found.");
            }
        }
        #endregion
    }
}