using NovaaTest.Enums;
using UnityEngine;

namespace NovaaTest.Controllers
{
    public class MainSceneController : MonoBehaviour
    {
        private void Start()
        {
            LoadMainMenu();
        }

        void LoadMainMenu()
        {
            SceneLoaderController.Instance.LoadScene(SceneType.MainMenu);
        }
    }
}
