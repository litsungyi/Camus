using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Camus.EditorHelpers
{
    public static class GizmoHelper
    {
        public static void DrawCollider(Collider collider, Color gizmosColor)
        {
            if (collider == null)
            {
                return;
            }

            if (collider is BoxCollider boxCollider)
            {
                Gizmos.color = gizmosColor;
                Gizmos.matrix = collider.transform.localToWorldMatrix;

                Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
            }
            else if (collider is SphereCollider sphereCollider)
            {
                Gizmos.color = gizmosColor;
                Gizmos.matrix = Matrix4x4.TRS(collider.transform.position, collider.transform.rotation, Vector3.one);

                var center = new Vector3(sphereCollider.center.x * collider.transform.lossyScale.x, sphereCollider.center.y * collider.transform.lossyScale.y, sphereCollider.center.z * collider.transform.lossyScale.z);
                var size = Mathf.Max(collider.transform.lossyScale.x, collider.transform.lossyScale.y, collider.transform.lossyScale.z) * sphereCollider.radius;
                Gizmos.DrawWireSphere(center, size);
            }
        }

        public static void DrawSolidCollider(Collider collider, Color gizmosColor)
        {
            if (collider == null)
            {
                return;
            }

            if (collider is BoxCollider)
            {
                Gizmos.color = gizmosColor;
                Gizmos.matrix = collider.transform.localToWorldMatrix;

                var boxCollider = collider as BoxCollider;
                Gizmos.DrawCube(boxCollider.center, boxCollider.size);
            }
            else if (collider is SphereCollider)
            {
                Handles.color = gizmosColor;
                Gizmos.matrix = Matrix4x4.TRS(collider.transform.position, collider.transform.rotation, Vector3.one);

                var sphereCollider = collider as SphereCollider;
                var center = new Vector3(sphereCollider.center.x * collider.transform.lossyScale.x, sphereCollider.center.y * collider.transform.lossyScale.y, sphereCollider.center.z * collider.transform.lossyScale.z);
                var size = Mathf.Max(collider.transform.lossyScale.x, collider.transform.lossyScale.y, collider.transform.lossyScale.z) * sphereCollider.radius;
                Handles.DrawSolidDisc(sphereCollider.transform.position + center, Vector3.up, size);
            }
        }

        public static void DrawLabel(string content, Vector3 position, int fontSize, Color fontColor, Texture2D background)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = fontColor;
            style.normal.background = background;
            style.fontSize = fontSize;
            style.padding = new RectOffset();
            style.margin = new RectOffset();
            Handles.Label(position, content, style);
        }

        public static void DrawDisc(Transform transform, float radius, Color color, Vector3 normal)
        {
            Handles.color = color;
            Gizmos.matrix = transform.localToWorldMatrix;
            var size = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z) * radius;
            Handles.DrawWireDisc(transform.position, normal, size);
        }

        public static void DrawSolidDisc(Transform transform, float size, Color color, Vector3 normal)
        {
            Handles.color = color;
            Gizmos.matrix = transform.localToWorldMatrix;
            Handles.DrawSolidDisc(transform.position, normal, size);
        }

        public static void DrawIcon(Vector3 position, string icon)
        {
            Gizmos.DrawIcon(position, icon);
        }

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
    }
}
