using System;
using UnityEngine;

namespace Camus.Timers
{
    public class TimerFixedUpdater : MonoBehaviour
    {
        [SerializeField] private TimerBase setting;
        private Action onTimer = null;

        private void Awake()
        {
            setting.Init(this);
        }

        private void FixedUpdate()
        {
            if (setting.Tick(Time.fixedDeltaTime))
            {
                Debug.Log("FixedUpdate");
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
