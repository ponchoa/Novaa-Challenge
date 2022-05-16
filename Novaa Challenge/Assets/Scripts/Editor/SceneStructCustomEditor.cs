using UnityEngine;
using UnityEditor;
using NovaaTest.Structs;

namespace NovaaTest.CustomInspector
{
    [CustomPropertyDrawer(typeof(SceneStruct))]
    public class SceneStructCustomEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // The rects that will determine the positions of the properties in SceneStruct.
            var sceneRect = new Rect(position.x, position.y, position.xMax - 115, position.height);
            var typeRect = new Rect(position.xMax - 65, position.y, 65, position.height);

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(sceneRect, property.FindPropertyRelative("sceneObject"), GUIContent.none);
            if (EditorGUI.EndChangeCheck()) // If there was any change.
            {
                UpdateCorrectSceneName(property);
            }
            EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("scenetype"), GUIContent.none);

            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Updates the sceneName property of the SceneStruct to reflect the sceneObject assigned.
        /// </summary>
        /// <param name="property">The SceneStruc to update.</param>
        void UpdateCorrectSceneName(SerializedProperty property)
        {
            // We need to update the SceneStruct early to be able to get the scene name.
            property.serializedObject.ApplyModifiedProperties();

            // We get the name of the scene object.
            Object sceneObject = property.FindPropertyRelative("sceneObject").objectReferenceValue;
            string sceneName = sceneObject is null ? "" : sceneObject.name;

            // We set the sceneName to the correct name. That is because on build, Unity doesn't keep the scene files,
            // so we have to cache the name of the scene, and that is the variable we use in the rest of the project.
            property.FindPropertyRelative("sceneName").stringValue = sceneName;
        }
    }
}