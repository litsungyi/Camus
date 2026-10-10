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
        // Gap in pixels between the gizmo border and the label
        private const int LabelGap = 2;

        // ponytail: half of a typical 32px icon, measure the drawn icon if labels overlap it
        private const int IconHalfHeight = 16;
#endif

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
                var position = GetLabelPosition();
                var offsetY = gizmoType == GizmoType.Icon ? IconHalfHeight + LabelGap : LabelGap;
                GizmoHelper.DrawLabel(label, position, labelFontSize, labelFontColor, null, offsetY);
            }
        }

        // The lowest point of the gizmo on screen, so the label sits below its border
        private Vector3 GetLabelPosition()
        {
            var camera = Camera.current;
            var down = camera != null ? -camera.transform.up : Vector3.down;

            switch (gizmoType)
            {
                case GizmoType.Collider:
                case GizmoType.SolidCollider:
                    if (thisCollider == null)
                    {
                        return transform.position;
                    }

                    var bounds = thisCollider.bounds;
                    var extents = bounds.extents;
                    var depth = extents.x * Mathf.Abs(down.x) + extents.y * Mathf.Abs(down.y) + extents.z * Mathf.Abs(down.z);
                    return bounds.center + down * depth;

                case GizmoType.Disc:
                case GizmoType.SolidDisc:
                    // Discs lie flat (normal up), so their lowest point is along down projected on the disc plane
                    var discRadius = GizmoHelper.GetMaxScale(transform) * radius;
                    return transform.position + Vector3.ProjectOnPlane(down, Vector3.up).normalized * discRadius;

                default:
                    return transform.position;
            }
        }
#endif
    }
}
