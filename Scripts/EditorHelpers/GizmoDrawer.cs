using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camus.EditorHelpers
{
    public class GizmoDrawer : MonoBehaviour
    {
        [SerializeField]
        private GizmoType gizmoType;

        [SerializeField]
        private string label;

        [SerializeField, Min(1)]
        private int labelFontSize = 12;

        [SerializeField]
        private Color labelFontColor = Color.blue;

        [SerializeField]
        private Collider thisCollider;

        [SerializeField]
        private Color gizmoColor = Color.white;

        [SerializeField]
        private Color gizmoSelectedColor = Color.red;

        [SerializeField, Min(0.01f)]
        private float radius = 1.0f;

        [SerializeField]
        private string icon;

        private void Start()
        {}

#if UNITY_EDITOR
        // Draw once with the selected color, OnDrawGizmosSelected would draw a second time on top
        private void OnDrawGizmos()
        {
            if (enabled)
            {
                DrawGizmo(IsSelected());
            }
        }

        // Matches OnDrawGizmosSelected: this object or one of its parents is selected
        private bool IsSelected()
        {
            foreach (var selected in UnityEditor.Selection.transforms)
            {
                if (transform.IsChildOf(selected))
                {
                    return true;
                }
            }

            return false;
        }

        private void DrawGizmo(bool selected)
        {
            var color = selected ? gizmoSelectedColor : gizmoColor;
            switch (gizmoType)
            {
                case GizmoType.Collider:
                    if (thisCollider != null)
                    {
                        GizmoHelper.DrawCollider(thisCollider, color);
                    }
                    break;

                case GizmoType.SolidCollider:
                    if (thisCollider != null)
                    {
                        GizmoHelper.DrawSolidCollider(thisCollider, color);
                    }
                    break;

                case GizmoType.Disc:
                    GizmoHelper.DrawDisc(transform, radius, color, Vector3.up);
                    break;

                case GizmoType.SolidDisc:
                    GizmoHelper.DrawSolidDisc(transform, radius, color, Vector3.up);
                    break;

                case GizmoType.Icon:
                    if (!string.IsNullOrEmpty(icon))
                    {
                        GizmoHelper.DrawIcon(transform.position, icon);
                    }
                    break;
                
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(label))
            {
                GizmoHelper.DrawLabel(label, transform.position, labelFontSize, labelFontColor, null);
            }
        }
#endif
    }
}
