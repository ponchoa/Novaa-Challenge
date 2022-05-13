using NovaaTest.SCObjects;
using UnityEditor;
using UnityEngine;

namespace NovaaTest.CustomInspector
{
    [CustomEditor(typeof(ScenesDataScriptableObject))]
    public class ScenesDataCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ScenesDataScriptableObject database = (ScenesDataScriptableObject)target;
            if (database != null)
            {
                if (GUILayout.Button("Resync scenes names"))
                {
                    ResyncScenesNames(database);
                }

                CheckScenes(database);
            }
        }

        /// <summary>
        /// Resyncs the cached scenes names with every Object attached the the ScenesDataScriptableObject.
        /// </summary>
        /// <param name="database">The ScenesDataScriptableObject to inspect.</param>
        void ResyncScenesNames(ScenesDataScriptableObject database)
        {
            if (database.scenesData is null)
                return;
            for (int i = 0; i < database.scenesData.Length; i++)
            {
                if (database.scenesData[i].sceneObject != null)
                {
                    database.scenesData[i].sceneName = database.scenesData[i].sceneObject.name;
                }
                else
                {
                    database.scenesData[i].sceneName = "";
                }
            }
        }
        /// <summary>
        /// Checks if the scenes are properly assigned in the inspector.
        /// </summary>
        /// <param name="database">The ScenesDataScriptableObject to inspect.</param>
        void CheckScenes(ScenesDataScriptableObject database)
        {
            if (database.scenesData is null)
                return;
            for (int i = 0; i < database.scenesData.Length; i++)
            {
                // This flag avoids displaying the same warning multiple times in the inspector.
                bool shouldBreak = false;
                if (database.scenesData[i].sceneObject is null)
                {
                    EditorGUILayout.HelpBox($"The scene at index {i} is empty", MessageType.Error);
                }
                else
                {
                    for (int j = i + 1; j < database.scenesData.Length; j++)
                    {
                        if (database.scenesData[i].scenetype == database.scenesData[j].scenetype)
                        {
                            shouldBreak = true;
                            EditorGUILayout.HelpBox("The same scene type was set multiple times", MessageType.Error);
                            break;
                        }
                    }
                }
                if (shouldBreak)
                    break;
            }
        }
    }
}