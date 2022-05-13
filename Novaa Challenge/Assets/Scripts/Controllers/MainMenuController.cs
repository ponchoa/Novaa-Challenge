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
            GameStateController.Instance?.LoadNextState();
        }
    }
}
