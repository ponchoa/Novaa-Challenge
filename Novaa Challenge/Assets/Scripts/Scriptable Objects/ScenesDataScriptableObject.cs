using System.Collections;
using System.Collections.Generic;
using NovaaTest.Structs;
using UnityEngine;

namespace NovaaTest.SCObjects
{

    [CreateAssetMenu(fileName = "Scenes Data", menuName = "Scene Data/Database", order = 1)]
    public class ScenesDataScriptableObject : ScriptableObject
    {
        [Tooltip("Every UI scenes and their type")]
        public SceneStruct[] scenesData;
    }
}