using Camus.Updatables;

namespace Camus.Timers
{
    public sealed class UpdatableTimer : TimerUpdatableBase, IUpdatable
    {
        void IUpdatable.OnUpdate(float delta)
        {
            TickAndTrigTimer(delta);
        }
    }
}
