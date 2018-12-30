namespace Camus.Timers
{
    public abstract class TimerUpdatableBase : TimerBase
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