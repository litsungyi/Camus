#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
#endif

namespace Camus.EditorHelpers
{
    public static class GizmoHelper
    {
#if UNITY_EDITOR
        public static void DrawCollider(Collider collider, Color gizmosColor)
        {
            DrawCollider(collider, gizmosColor, false);
        }

        public static void DrawSolidCollider(Collider collider, Color gizmosColor)
        {
            DrawCollider(collider, gizmosColor, true);
        }

        private static void DrawCollider(Collider collider, Color gizmosColor, bool solid)
        {
            if (collider == null)
            {
                return;
            }

            var transform = collider.transform;
            var matrix = Gizmos.matrix;
            Gizmos.color = gizmosColor;

            if (collider is BoxCollider boxCollider)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                if (solid)
                {
                    Gizmos.DrawCube(boxCollider.center, boxCollider.size);
                }
                else
                {
                    Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
                }
            }
            else if (collider is SphereCollider sphereCollider)
            {
                // A sphere collider stays round, Unity scales its radius by the largest axis
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
                var center = Vector3.Scale(sphereCollider.center, transform.lossyScale);
                var radius = GetMaxScale(transform) * sphereCollider.radius;
                if (solid)
                {
                    Gizmos.DrawSphere(center, radius);
                }
                else
                {
                    Gizmos.DrawWireSphere(center, radius);
                }
            }

            Gizmos.matrix = matrix;
        }

        // Centers the label horizontally with its top at position, pushed down by offsetY pixels
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void DrawLabel(string content, Vector3 position, int fontSize, Color fontColor, Texture2D background, int offsetY = 0)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = fontColor;
            style.normal.background = background;
            style.fontSize = fontSize;
            style.alignment = TextAnchor.UpperCenter;
            style.padding = new RectOffset(0, 0, offsetY, 0);
            style.margin = new RectOffset();
            Handles.Label(position, content, style);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void DrawDisc(Transform transform, float radius, Color color, Vector3 normal)
        {
            Handles.color = color;
            Handles.DrawWireDisc(transform.position, normal, GetMaxScale(transform) * radius);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void DrawSolidDisc(Transform transform, float radius, Color color, Vector3 normal)
        {
            Handles.color = color;
            Handles.DrawSolidDisc(transform.position, normal, GetMaxScale(transform) * radius);
        }

        internal static float GetMaxScale(Transform transform)
        {
            var scale = transform.lossyScale;
            return Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void DrawIcon(Vector3 position, string icon)
        {
            Gizmos.DrawIcon(position, icon);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void DrawPath(IList<Vector3> positions, Color color, int width, float fixHeight)
        {
            Gizmos.color = color;
            for (int i = 0; i < positions.Count - 1; i++)
            {
                var from = positions[i];
                var to = positions[i + 1];
                DrawLine(new Vector3(from.x, fixHeight, from.z), new Vector3(to.x, fixHeight, to.z), width);
            }
        }

        // Ref: http://answers.unity3d.com/questions/1139985/gizmosdrawline-thickens.html
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 p1, Vector3 p2, int width)
        {
            if (width <= 1)
            {
                Gizmos.DrawLine(p1, p2);
            }
            else
            {
                Camera c = Camera.current;
                if (c == null)
                {
                    Gizmos.DrawLine(p1, p2);
                    Debug.LogError("Camera.current is null");
                    return;
                }

                Vector3 v1 = (p2 - p1).normalized; // line direction
                Vector3 v2 = (c.transform.position - p1).normalized; // direction to camera
                Vector3 n = Vector3.Cross(v1,v2); // normal vector
                n.y = 0;
                for (int i = 0; i < width; i++)
                {
                    Vector3 o = (n* ((float)i/(width-1) - 0.5f)*0.25f);
                    Gizmos.DrawLine(p1+o,p2+o);
                }
            }
        }
#endif
    }
}
