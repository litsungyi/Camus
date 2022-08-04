using Camus.Updatables;

namespace Camus.Timers
{
    public sealed class FixedUpdatableTimer : TimerUpdatableBase, IFixedUpdatable
    {
        void IFixedUpdatable.OnFixedUpdate(float duration)
        {
            TickAndTrigTimer(duration);
        }
    }
}
