using NovaaTest.Enums;
using UnityEngine;

namespace NovaaTest.Controllers
{
    public class MainSceneController : MonoBehaviour
    {
        private void Start()
        {
            GameStateController.Instance?.InitializeGame();
        }
    }
}
