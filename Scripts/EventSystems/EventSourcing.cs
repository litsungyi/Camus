using System;

namespace Camus.EventSystems
{
    public class EventSourcing
    {
        private static class EventHost<T> where T : IEvent
        {
            public static event Action<T> OnRaise = delegate { };

            public static void DoRaise(T args)
            {
                OnRaise(args);
            }
        }

        public void Register<T>(Action<T> callback) where T : IEvent
        {
            EventHost<T>.OnRaise += callback;
        }

        public void Unregister<T>(Action<T> callback) where T : IEvent
        {
            EventHost<T>.OnRaise -= callback;
        }

        public void Raise<T>(T args) where T : IEvent
        {
            EventHost<T>.DoRaise(args);
        }
    }
}
