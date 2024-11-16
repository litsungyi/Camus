using UnityEngine;

namespace Camus.Timers
{
    public sealed class LateUpdateTimer : TimerBase
    {
        private void LateUpdate()
        {
            TickAndTrigTimer(Time.deltaTime);
        }
    }
}
