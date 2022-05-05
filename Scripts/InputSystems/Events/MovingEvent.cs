using Camus.EventSystems;
using UnityEngine;

namespace Camus.Inputs.Events
{
    public class MovingEvent : IDomainEvent
    {
        public MovingEvent(InputManager.Button button, Vector3 position, Vector3 direction, Vector3 distance)
        {
            Button = button;
            Position = position;
            Direction = direction;
            Distance = distance;
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

        public Vector3 Direction
        {
            get;
            private set;
        }

        public Vector3 Distance
        {
            get;
            private set;
        }
    }
}
