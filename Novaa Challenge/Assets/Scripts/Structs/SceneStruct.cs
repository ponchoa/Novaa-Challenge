using NovaaTest.Enums;
using UnityEngine;

namespace NovaaTest.Structs
{
    [System.Serializable]
    public struct SceneStruct
    {
        /// <summary>
        /// DO NOT USE. It is only to set the sceneName property. Use that instead.
        /// </summary>
        public Object sceneObject;
        /// <summary>
        /// The name of the scene.
        /// </summary>
        public string sceneName;
        /// <summary>
        /// The type of the scene.
        /// </summary>
        public GameState scenetype;
    }
}