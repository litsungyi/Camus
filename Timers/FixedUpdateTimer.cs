using UnityEngine;

namespace Camus.Timers
{
    public sealed class FixedUpdateTimer : TimerBase
    {
        private void FixedUpdate()
        {
            TickAndTrigTimer(Time.fixedDeltaTime);
        }
    }
}
