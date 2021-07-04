using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Camus.EditorHelpers
{
    [CustomEditor(typeof(GizmoDrawer))]
    public class GizmoDrawerInspector : BaseInspector 
    {
        private bool showLabel = true;
        private bool showGizmo = true;
        private SerializedProperty gizmoTypeField;
        private SerializedProperty labelField;
        private SerializedProperty labelFontSizeField;
        private SerializedProperty labelFontColorField;
        private SerializedProperty colliderField;
        private SerializedProperty gizmoColorField;
        private SerializedProperty gizmoSelectedColorField;
        private SerializedProperty radiusField;
        private SerializedProperty iconField;

        protected override void OnEnableInspector()
        {
            gizmoTypeField = serializedObject.FindProperty("gizmoType");
            labelField = serializedObject.FindProperty("label");
            labelFontSizeField = serializedObject.FindProperty("labelFontSize");
            labelFontColorField = serializedObject.FindProperty("labelFontColor");
            colliderField = serializedObject.FindProperty("thisCollider");
            gizmoColorField = serializedObject.FindProperty("gizmoColor");
            gizmoSelectedColorField = serializedObject.FindProperty("gizmoSelectedColor");
            radiusField = serializedObject.FindProperty("radius");
            iconField = serializedObject.FindProperty("icon");
        }

        protected override bool OnDrawInspector()
        {
			EditorGUI.BeginChangeCheck();
            var drawer = target as GizmoDrawer;

            var gizmoType = DrawType();
            DrawLabel();
            DrawGizmo(gizmoType);

            return true;

            GizmoType DrawType()
            {
                var type = (GizmoType) gizmoTypeField.enumValueIndex;
                gizmoTypeField.enumValueIndex = (int) (GizmoType) EditorGUILayout.EnumPopup("Gizmo Type:", type);

                return type;
            }

            void DrawLabel()
            {
                showLabel = EditorGUILayout.BeginFoldoutHeaderGroup(showLabel, "Label");
                if (showLabel)
                {
                    labelField.stringValue = EditorGUILayout.TextField("Label", labelField.stringValue);
                    labelFontSizeField.intValue = EditorGUILayout.IntField("Label Font Size", labelFontSizeField.intValue);
                    labelFontColorField.colorValue = EditorGUILayout.ColorField("Label Font Color", labelFontColorField.colorValue);
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            void DrawGizmo(GizmoType type)
            {
                showGizmo = EditorGUILayout.BeginFoldoutHeaderGroup(showGizmo, "Gizmo");
                if (!showGizmo)
                {
                    EditorGUILayout.EndFoldoutHeaderGroup();
                    return;
                }

                switch (type)
                {
                    case GizmoType.Collider:
                    case GizmoType.SolidCollider:
                        colliderField.objectReferenceValue = EditorGUILayout.ObjectField("Collider", colliderField.objectReferenceValue, typeof(Collider), true);
                        gizmoColorField.colorValue = EditorGUILayout.ColorField("Gizmo Color", gizmoColorField.colorValue);
                        gizmoSelectedColorField.colorValue = EditorGUILayout.ColorField("Gizmo Color", gizmoSelectedColorField.colorValue);
                        break;

                    case GizmoType.Disc:
                    case GizmoType.SolidDisc:
                        gizmoColorField.colorValue = EditorGUILayout.ColorField("Gizmo Color", gizmoColorField.colorValue);
                        gizmoSelectedColorField.colorValue = EditorGUILayout.ColorField("Gizmo Color", gizmoSelectedColorField.colorValue);
                        radiusField.floatValue = EditorGUILayout.FloatField("Radius", radiusField.floatValue);
                        break;

                    case GizmoType.Icon:
                        iconField.stringValue = EditorGUILayout.TextField("Icon", iconField.stringValue);
                        break;
                    
                    default:
                        break;
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }
    }
}
