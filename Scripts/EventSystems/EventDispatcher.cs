using System;

namespace Campus.EventSystems
{
    public static class EventDispatcher
    {
        private static class EventHost<T> where T: IDomainEvent
        {
            public static event Action<T> OnRaise = delegate {};

            public static void DoRaise(T args)
            {
                OnRaise(args);
            }
        }

        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            EventHost<T>.OnRaise += callback;
        }

        public static void Unregister<T>(Action<T> callback) where T : IDomainEvent
        {
            EventHost<T>.OnRaise -= callback;
        }

        public static void Raise<T>(T args) where T : IDomainEvent
        {
            EventHost<T>.DoRaise(args);
        }
    }
}
