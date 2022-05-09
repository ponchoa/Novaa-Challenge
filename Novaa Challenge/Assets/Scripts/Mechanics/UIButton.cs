using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        if (buttonText is null) //We make sure that there is a text referenced
        {
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
            Debug.LogWarning("CategoryButton (" + name + ") : No text (TMP) reference was set in the inspector. The wrong text might have been found");
        }
    }
}
