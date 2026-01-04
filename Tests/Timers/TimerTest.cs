using UnityEngine;

namespace Camus.Timers
{
    public class TimerTest : MonoBehaviour
    {
        public FixedUpdateTimer fixedUpdateTimer;
        public UpdateTimer updateTimer;
        public LateUpdateTimer lateUpdateTimer;

        public FixedUpdatableTimer fixedUpdatableTimer;
        public UpdatableTimer updatableTimer;
        public LateUpdatableTimer lateUpdatableTimer;

        private void OnGUI()
        {
            ShowTimerControll("FixedUpdateTimer", fixedUpdateTimer);
            ShowTimerControll("UpdateTimer", updateTimer);
            ShowTimerControll("LateUpdateTimer", lateUpdateTimer);

            ShowTimerControll("FixedUpdatableTimer", fixedUpdatableTimer);
            ShowTimerControll("UpdatableTimer", updatableTimer);
            ShowTimerControll("LateUpdatableTimer", lateUpdatableTimer);
        }

        private void ShowTimerControll(string label, TimerBase timer)
        {
            GUILayout.BeginVertical();
            GUILayout.Label(label);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();

            var status = timer.Status;
            if (status == TimerBase.TimerStatus.None)
            {
                if (GUILayout.Button("Start"))
                {
                    timer.StartTimer(() => { Debug.Log(label + " OnTimer()"); });
                }
            }

            if (status != TimerBase.TimerStatus.None)
            {
                if (GUILayout.Button("Restart"))
                {
                    timer.RestartTimer();
                }
            }

            if (status == TimerBase.TimerStatus.Started)
            {
                if (GUILayout.Button("Stop"))
                {
                    timer.StopTimer();
                }
            }

            if (status == TimerBase.TimerStatus.Started)
            {
                if (GUILayout.Button("Pause"))
                {
                    timer.PauseTimer();
                }
            }

            if (status == TimerBase.TimerStatus.Paused)
            {
                if (GUILayout.Button("Resume"))
                {
                    timer.ResumeTimer();
                }
            }

            GUILayout.EndHorizontal();
        }
    }
}
