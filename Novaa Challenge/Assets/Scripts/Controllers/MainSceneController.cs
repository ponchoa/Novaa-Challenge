using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    private void Start()
    {
        SceneLoaderController.Instance.LoadScene(SceneType.MAINMENU); //We load the main menu on start for now
    }
}
