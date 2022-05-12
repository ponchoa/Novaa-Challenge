using NovaaTest.Enums;
using UnityEngine;

namespace NovaaTest.Controllers
{
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
}
