using Camus.EventSystems;
using UnityEngine;

namespace Camus.Inputs.Events
{
    public class BeginPressEvent : IDomainEvent
    {
        public BeginPressEvent(InputManager.Button button, Vector3 position)
        {
            Button = button;
            Position = position;
        }

        public InputManager.Button Button
        {
            get;
            private set;
        }

        public Vector3 Position
        {
            get;
            private set;
        }
    }
}
