using System;
using UnityEngine;

namespace Camus.Timers
{
    [Serializable]
    public class TimerBase
    {
        [Header("Display")]
        public string name;
        private MonoBehaviour host;

        [Header("Timer Setting")]
        public float interval;
        public bool loop;
        public float maxCount;

        [Header("Timer Setting")]
        [SerializeField] protected float duration;
        [SerializeField] protected float count;

        internal void Init(MonoBehaviour host)
        {
            this.host = host;
        }

        internal void StartTimer()
        {
            SetEnable(true);
            ResetTimer();
        }

        internal void StopTimer()
        {
            SetEnable(false);
            ResetTimer();
        }

        internal void PauseTimer()
        {
            SetEnable(false);
        }

        internal void ResumeTimer()
        {
            if (count >= maxCount && !loop)
            {
                return;
            }

            SetEnable(true);
        }

        internal void ResetTimer()
        {
            duration = 0;
            count = 0;
        }

        internal bool Tick(float delta)
        {
            duration += delta;
            if (duration < interval)
            {
                return false;
            }

            duration -= interval;
            if (++count >= maxCount && !loop)
            {
                SetEnable(false);
            }

            return true;
        }

        private void SetEnable(bool enabled)
        {
            host.enabled = enabled;
        }
    }
}
