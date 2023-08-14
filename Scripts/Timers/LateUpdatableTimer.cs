using Camus.Updatables;

namespace Camus.Timers
{
    public sealed class LateUpdatableTimer : TimerUpdatableBase, ILateUpdatable
    {
        void ILateUpdatable.OnLateUpdate(float duration)
        {
            TickAndTrigTimer(duration);
        }
    }
}
