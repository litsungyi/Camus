using System.Collections.Generic;
using UnityEngine;

namespace Camus.Utilities
{
    public class FPS : MonoBehaviour
    {
        private static readonly int MaxCount = 100;

        [SerializeField] private int count = 0;
        [SerializeField] private float frames = 0f;
        private Queue<float> deltas = new Queue<float>();

        private void Update()
        {
            var delta = Time.deltaTime;
            ++count;
            deltas.Enqueue(delta);

            if (deltas.Count > MaxCount)
            {
                deltas.Dequeue();
                count = MaxCount;
            }

            var total = 0f;
            foreach (var item in deltas)
            {
                total += item;
            }

            var average = count == -1 ? 0 : total / count;
            frames = average <= 0 ? 0 : 1f / average;
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            UnityEditor.EditorGUILayout.LabelField(string.Format("FPS: {0:00.0}", frames));
        }
#endif
    }
}
