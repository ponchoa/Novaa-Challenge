using NovaaTest.SCObjects;
using NovaaTest.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NovaaTest.Controllers
{
    /// <summary>
    /// This is a Singleton class that helps transitioning from one game state to another.
    /// </summary>
    public class GameStateController : MonoBehaviour
    {
        #region Singleton
        private static GameStateController instance;
        public static GameStateController Instance
        {
            get
            {
                if (instance is null)
                    instance = FindObjectOfType<GameStateController>();
                return instance;
            }
        }
        #endregion

        [SerializeField]
        [Tooltip("The scenes data scriptable object that will govern the scenes of this game")]
        ScenesDataScriptableObject sceneDatabase;

        GameState currentState;

        private void Awake()
        {
            MakeInstance();
        }
        private void Start()
        {
            CheckSceneDatabase();
        }

        #region Checks
        /// <summary>
        /// Checks if the scenes data scriptable object is properly set.
        /// </summary>
        void CheckSceneDatabase()
        {
            if (sceneDatabase is null)
            {
                Debug.LogError($"GameStateController ({name}) : No scene database was referenced in the inspector.", this);
            }
            else if (sceneDatabase.scenesData is null || sceneDatabase.scenesData.Length <= 0)
            {
                Debug.LogError($"GameStateController ({name}) : The scene database {sceneDatabase.name} is not set up correctly.", this);
            }
        }
        #endregion

        /// <summary>
        /// Initializes the singleton instance.
        /// </summary>
        void MakeInstance()
        {
            if (instance is null)
                instance = this;
        }

        /// <summary>
        /// This should only be called once at the very beginning of the game, and initializes the game.
        /// (Call ResetGame() instead of this after the game has begun to restart.)
        /// </summary>
        public void InitializeGame()
        {
            currentState = GameState.MainMenu;
            SceneLoaderController.Instance.Initialize(sceneDatabase.scenesData);
            SceneLoaderController.Instance.LoadScene(GameState.MainMenu);
        }

        /// <summary>
        /// Ends the current state and loads the next one. Will load and unload the correct scenes.
        /// </summary>
        public void LoadNextState()
        {
            GameState nextState = GetNextState(currentState);

            if (SceneLoaderController.Instance.LoadScene(nextState))
            {
                SceneLoaderController.Instance.UnloadScene(currentState);
                currentState = nextState;
            }
        }

        /// <summary>
        /// Call to resets the game to its starting state.
        /// </summary>
        public void ResetGame()
        {
            if (SceneLoaderController.Instance.LoadScene(GameState.MainMenu))
            {
                SceneLoaderController.Instance.UnloadScene(currentState);
                currentState = GameState.MainMenu;
            }
        }

        /// <summary>
        /// Returns the logical next game stats depending on the one passed as parameter.
        /// </summary>
        /// <param name="current">The state that precedes the one we want to get.</param>
        /// <returns>The state that comes after the current parameter.</returns>
        GameState GetNextState(GameState current)
        {
            switch (current)
            {
                default:
                case GameState.Results:
                case GameState.MainMenu:
                    return GameState.Categories;
                case GameState.Categories:
                    return GameState.Quiz;
                case GameState.Quiz:
                    return GameState.Results;
            }
        }
    }
}