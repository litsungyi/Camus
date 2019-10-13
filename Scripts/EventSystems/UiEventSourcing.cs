using System;

namespace Camus.EventSystems
{
    public class UiEventSourcing
    {
        private static class EventHost<T> where T : IUiEvent
        {
            public static event Action<T> OnRaise = delegate { };

            public static void DoRaise(T args)
            {
                OnRaise(args);
            }
        }

        public void Register<T>(Action<T> callback) where T : IUiEvent
        {
            EventHost<T>.OnRaise += callback;
        }

        public void Unregister<T>(Action<T> callback) where T : IUiEvent
        {
            EventHost<T>.OnRaise -= callback;
        }

        public void Raise<T>(T args) where T : IUiEvent
        {
            EventHost<T>.DoRaise(args);
        }
    }
}
