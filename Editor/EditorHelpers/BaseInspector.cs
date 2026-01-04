using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Camus.EditorHelpers
{
    public abstract class BaseInspector : Editor 
    {
        private SerializedProperty scriptProperty;

        private void OnEnable()
        {
            scriptProperty = serializedObject.FindProperty("m_Script");

            OnEnableInspector();
        }

        public sealed override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUI.enabled = false;
            EditorGUILayout.PropertyField(scriptProperty);
            GUI.enabled = true;

            var result = OnDrawInspector();
            if (result)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        protected abstract void OnEnableInspector();

        protected abstract bool OnDrawInspector();
    }
}
