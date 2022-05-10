using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    private void Start()
    {
        LoadMainMenu();
    }

    void LoadMainMenu()
    {
        SceneLoaderController.Instance.LoadScene(SceneType.MAINMENU);
    }
}
