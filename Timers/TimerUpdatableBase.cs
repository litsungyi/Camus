using System.Collections;

namespace Camus.Timers
{
    public class TimerUpdatableBase : TimerBase
    {
        private void OnEnable()
        {
            App.Instance.Updater.Register(this);
        }

        private void OnDisable()
        {
            App.Instance.Updater.Unregister(this);
        }
    }
}