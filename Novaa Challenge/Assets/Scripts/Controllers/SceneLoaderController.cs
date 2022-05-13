using NovaaTest.Enums;
using NovaaTest.Structs;
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

        /// <summary>
        /// Maps each scene type to a corresponding scene name
        /// </summary>
        Dictionary<GameState, string> scenesMap;

        private void Awake()
        {
            MakeInstance();
        }

        void MakeInstance()
        {
            if (instance is null)
                instance = this;
        }

        /// <summary>
        /// Initializes all of the scenes that will be used in-game. (Remember to add them to the Build Settings)
        /// </summary>
        /// <param name="scenesArray">The array containing the data pertaining to each scene</param>
        public void Initialize(SceneStruct[] scenesArray)
        {
            scenesMap = new Dictionary<GameState, string>();
            if (scenesArray != null)
            {
                for (int i = 0; i < scenesArray.Length; i++)
                {
                    scenesMap.Add(scenesArray[i].scenetype, scenesArray[i].sceneName);
                }
            }
        }

        /// <summary>
        /// Loads additively the scene specified.
        /// </summary>
        /// <param name="scene">The scene type that needs to be loaded</param>
        /// <returns>True if the scene was loaded, false if it wasn't.</returns>
        public bool LoadScene(GameState scene)
        {
            if (scenesMap.ContainsKey(scene))
            {
                //TODO: Load scene asynchronously to avoid eventual stutters.
                SceneManager.LoadScene(scenesMap[scene], LoadSceneMode.Additive);
                return true;
            }
            Debug.LogError($"SceneLoaderController({name}) : A scene of an unknown type tried to be loaded. Add the corresponding scene type in the controller", this);
            return false;
        }

        /// <summary>
        /// Unloads a loaded scene.
        /// </summary>
        /// <param name="scene">The scene type that was previously loaded additively.</param>
        /// <returns>Whether or not the operation succeded.</returns>
        public bool UnloadScene(GameState scene)
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