// Ref. https://gist.github.com/LotteMakesStuff/c0a3b404524be57574ffa5f8270268ea

using UnityEditor;
using UnityEngine;

namespace Camus.Utilities
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool wasEnabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, true);
            GUI.enabled = wasEnabled;
        }
    }
}
