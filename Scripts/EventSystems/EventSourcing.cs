using System;
using System.Collections.Generic;

namespace Camus.EventSystems
{
    // Instance-scoped: each EventSourcing has its own subscribers (unlike the global EventDispatcher).
    public class EventSourcing
    {
        private readonly Dictionary<Type, Delegate> handlers = new Dictionary<Type, Delegate>();

        public void Register<T>(Action<T> callback) where T : IEvent
        {
            handlers.TryGetValue(typeof(T), out var handler);
            handlers[typeof(T)] = Delegate.Combine(handler, callback);
        }

        public void Unregister<T>(Action<T> callback) where T : IEvent
        {
            if (handlers.TryGetValue(typeof(T), out var handler))
            {
                handlers[typeof(T)] = Delegate.Remove(handler, callback);
            }
        }

        public void Raise<T>(T args) where T : IEvent
        {
            if (handlers.TryGetValue(typeof(T), out var handler))
            {
                (handler as Action<T>)?.Invoke(args);
            }
        }
    }
}
