using UnityEngine;

namespace Camus.Timers
{
    public sealed class UpdateTimer : TimerBase
    {
        private void Update()
        {
            TickAndTrigTimer(Time.deltaTime);
        }
    }
}
