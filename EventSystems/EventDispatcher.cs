using System;

namespace Campus.EventSystems
{
    public static class EventDispatcher
    {
        private static class EventHost<T>
        {
            public static event Action<T> OnRaise = delegate {};

            public static void DoRaise(T args)
            {
                OnRaise(args);
            }
        }

        public static void Register<T>(Action<T> callback)
        {
            EventHost<T>.OnRaise += callback;
        }

        public static void Unregister<T>(Action<T> callback)
        {
            EventHost<T>.OnRaise -= callback;
        }

        public static void Raise<T>(T args)
        {
            EventHost<T>.DoRaise(args);
        }
    }
}
