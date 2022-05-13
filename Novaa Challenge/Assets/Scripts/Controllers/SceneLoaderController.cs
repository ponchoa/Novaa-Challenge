using NovaaTest.Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NovaaTest.Controllers
{
    /// <summary>
    /// This is a Singleton class that helps loading and unloading UI scenes.
    /// </summary>
    public class SceneLoaderController : MonoBehaviour
    {
        #region Singleton
        private static SceneLoaderController instance;
        public static SceneLoaderController Instance
        {
            get
            {
                if (instance is null)
                    instance = FindObjectOfType<SceneLoaderController>();
                return instance;
            }
        }
        #endregion

        //TODO: add a better way to manage scenes
        [SerializeField]
        [Tooltip("The name of the Main Menu Scene")]
        string mainMenuScene;
        [SerializeField]
        [Tooltip("The name of the Categories Scene")]
        string categoriesScene;
        [SerializeField]
        [Tooltip("The name of the Quiz Scene")]
        string quizScene;
        [SerializeField]
        [Tooltip("The name of the Results Scene")]
        string resultsScene;

        /// <summary>
        /// Maps each scene type to a corresponding scene name
        /// </summary>
        Dictionary<SceneType, string> scenesMap;

        private void Awake()
        {
            MakeInstance();
            FillScenesMap();
        }

        void MakeInstance()
        {
            if (instance is null)
                instance = this;
        }

        void FillScenesMap()
        {
            //TODO: For now we manually set up the map, but it should be changed.
            scenesMap = new Dictionary<SceneType, string>();
            scenesMap.Add(SceneType.MainMenu, mainMenuScene);
            scenesMap.Add(SceneType.Categories, categoriesScene);
            scenesMap.Add(SceneType.Quiz, quizScene);
            scenesMap.Add(SceneType.Results, resultsScene);
        }

        /// <summary>
        /// Loads additively the scene specified.
        /// </summary>
        /// <param name="scene">The scene type that needs to be loaded</param>
        /// <returns>True if the scene was loaded, false if it wasn't.</returns>
        public bool LoadScene(SceneType scene)
        {
            if (scenesMap.ContainsKey(scene))
            {
                //TODO: Load scene asynchronously to avoid eventual stutters.
                SceneManager.LoadScene(scenesMap[scene], LoadSceneMode.Additive);
                return true;
            }
            Debug.LogWarning($"SceneLoaderController({name}) : A scene of an unknown type tried to be loaded. Add the corresponding scene type in the controller", this);
            return false;
        }

        /// <summary>
        /// Unloads a loaded scene.
        /// </summary>
        /// <param name="scene">The scene type that was previously loaded additively.</param>
        /// <returns>Whether or not the operation succeded.</returns>
        public bool UnloadScene(SceneType scene)
        {
            if (scenesMap.ContainsKey(scene))
            {
                if (SceneManager.GetSceneByName(scenesMap[scene]).IsValid())
                {
                    //TODO: Actually use the Async Operation to check whether the scene is unloaded before continuing.
                    SceneManager.UnloadSceneAsync(scenesMap[scene]);
                    return true;
                }
                Debug.LogWarning($"SceneLoaderController({name}) : The scene \"{scenesMap[scene]}\" tried to be unloaded when it wasn't loaded beforehand", this);
                return false;
            }
            Debug.LogWarning($"SceneLoaderController({name}) : A scene of an unknown type tried to be unloaded. Add the corresponding scene type in the controller.", this);
            return false;
        }
    }
}