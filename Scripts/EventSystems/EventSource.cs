using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Camus.EventSystems
{
    public class EventSource<T> where T : IEvent
    {
        private IDictionary<Type, IList> eventDispatcher = new Dictionary<Type, IList>();

        public void Register<TAction>(Action<TAction> action) where TAction : T
        {
            IList actions = null;
            if (eventDispatcher.TryGetValue(typeof(TAction), out actions))
            {
                actions.Add(action);
            }
            else
            {
                actions = new List<Action<TAction>>
                {
                    action
                };
                eventDispatcher.Add(typeof(T), actions);
            }
        }

        public void Unregister<TAction>(Action<TAction> action) where TAction : T
        {
            IList actions = null;
            if (eventDispatcher.TryGetValue(typeof(T), out actions))
            {
                actions.Remove(action);
            }
        }

        public void Raise<TAction>(TAction @event) where TAction : T
        {
            IList actions = null;
            if (eventDispatcher.TryGetValue(typeof(T), out actions))
            {
                foreach (Action<TAction> action in actions)
                {
                    action(@event);
                }
            }
        }
    }
}
