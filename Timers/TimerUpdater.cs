using System;
using UnityEngine;

namespace Camus.Timers
{
    public class TimerUpdater : MonoBehaviour
    {
        [SerializeField] private TimerBase setting;
        private Action onTimer = null;

        private void Awake()
        {
            setting.Init(this);
        }

        private void Update()
        {
            if (setting.Tick(Time.deltaTime))
            {
                Debug.Log("Update");
                onTimer?.Invoke();
            }
        }

        public void StartTimer()
        {
            setting.StartTimer();
        }

        public void StopTimer()
        {
            setting.StopTimer();
        }

        public void PauseTimer()
        {
            setting.PauseTimer();
        }

        public void ResumeTimer()
        {
            setting.ResumeTimer();
        }

        public void ResetTimer()
        {
            setting.ResetTimer();
        }
    }
}
