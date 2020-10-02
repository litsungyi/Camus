using System;
using Camus.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Camus.Timers
{
    public abstract class TimerBase : MonoBehaviour
    {
        #region Inner Classes

        [Serializable]
        public class TimerSetting
        {
            public float interval = 0f;
            public bool loop = false;
            public int maxCount = 0;

            public bool IsEnd(int count)
            {
                return !loop && count >= maxCount;
            }
        }

        [Serializable]
        private class TimerState
        {
            [SerializeField] private float duration = 0f;
            [SerializeField] private int count = 0;

            public int Count => count;

            public bool Tick(float delta, float interval)
            {
                duration += delta;
                if (duration >= interval)
                {
                    duration -= interval;
                    ++count;
                    return true;
                }

                return false;
            }

            public void Reset(float duration = 0, int count = 0)
            {
                this.duration = duration;
                this.count = count;
            }
        }

        #endregion

        #region Enum

        public enum TimerStatus
        {
            None,
            Started,
            Paused,
            Stopped,
        }

        #endregion

        #region Fields

        [SerializeField] private TimerSetting setting;
        [ReadOnly, SerializeField] private TimerState state = null;
        [ReadOnly, SerializeField] private TimerStatus status = TimerStatus.None;
        private Action onTimer = null;

        public TimerStatus Status
        {
            get => status;
            private set => status = value;
        }

        #endregion

        #region UpdateSetting

        public void UpdateSetting(TimerSetting setting)
        {
            this.setting = setting;
        }

        public void UpdateSetting(TimerSetting setting, Action onTimer)
        {
            UpdateSetting(setting);
            this.onTimer = onTimer;
        }

        #endregion

        #region Timer Controll

        public void StartTimer(Action onTimer = null)
        {
            StartTimer(0, onTimer);
        }

        public void StartTimer(float delta, Action onTimer = null)
        {
            this.onTimer = onTimer;
            RestartTimer(delta);
        }

        public void RestartTimer(float delta = 0)
        {
            SetEnable(true);
            state.Reset(delta);
            Status = TimerStatus.Started;
        }

        public void StopTimer()
        {
            SetEnable(false);
            state.Reset();
            Status = TimerStatus.Stopped;
        }

        public void PauseTimer()
        {
            SetEnable(false);
            Status = TimerStatus.Paused;
        }

        public void ResumeTimer()
        {
            if (setting.IsEnd(state.Count))
            {
                return;
            }

            SetEnable(true);
            Status = TimerStatus.Started;
        }

        #endregion

        #region Internal Function

        internal void TickAndTrigTimer(float delta)
        {
            if (Status == TimerStatus.None)
            {
                SetEnable(false);
                return;
            }

            if (Tick(delta))
            {
                TrigTimer();
            }
        }

        internal bool Tick(float delta)
        {
            if (!state.Tick(delta, setting.interval))
            {
                return false;
            }

            if (setting.IsEnd(state.Count))
            {
                StopTimer();
            }

            return true;
        }

        internal void TrigTimer()
        {
            onTimer?.Invoke();
        }

        private void SetEnable(bool enable)
        {
            this.enabled = enable;
        }

        #endregion

        #region Validation

        private void OnValidate()
        {
            Assert.IsTrue(setting.interval > 0, "Interval must > 0");
            Assert.IsTrue(setting.maxCount >= 0, "Max count must >= 0");
        }

        #endregion
    }
}
