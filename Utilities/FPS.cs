using System.Collections.Generic;
using UnityEngine;

namespace Camus.Utilities
{
    public class FPS : MonoBehaviour
    {
        private static readonly int MaxCount = 100;

        [SerializeField] private float frames = 0f;
        private Queue<float> deltas = new Queue<float>();

        private void Update()
        {
            var delta = Time.deltaTime;
            deltas.Enqueue(delta);

            if (deltas.Count > MaxCount)
            {
                deltas.Dequeue();
            }

            var total = 0f;
            foreach (var item in deltas)
            {
                total += item;
            }

            var average = deltas.Count == 0 ? 0 : total / deltas.Count;
            frames = average <= 0 ? 0 : 1f / average;
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            var style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = GetColor();
            style.fontSize = 16;
            style.alignment = TextAnchor.MiddleRight;

            var rect = new Rect(new Vector2(Screen.width - 110, 10), new Vector2(100, 20));
            UnityEditor.EditorGUI.LabelField(rect, string.Format("FPS: {0:00.0}", frames), style);
        }

        private Color GetColor()
        {
            if (frames >= 60f)
            {
                return Color.green;
            }
            else if (frames >= 30f)
            {
                return Color.blue;
            }
            else if (frames >= 10f)
            {
                return Color.yellow;
            }
            else
            {
                return Color.red;
            }
        }
#endif
    }
}
