using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// The start button listener
    /// </summary>
    public void OnStartButtonClick()
    {
        if (SceneLoaderController.Instance.LoadScene(SceneType.CATEGORIES))
        {
            SceneLoaderController.Instance.UnloadScene(SceneType.MAINMENU);
        }
    }
}
