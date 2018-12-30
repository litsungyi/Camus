using Camus.Updatables;
using UnityEngine;

namespace Camus.Inputs
{
    public class InputManager : IUpdatable
    {
        private enum Button
        {
            Primary = 0, // often the left button
            Secondary = 1,
            Middle = 2,
        }

        private Vector3 prevPosition;
        private Vector3 lastPosition;
        private Vector3 dragPosition;

        private bool dragging = false;

        public Vector3 delta
        {
            get
            {
                return lastPosition - prevPosition;
            }
        }

        public Vector3 dragDelta
        {
            get
            {
                return dragging ? dragPosition - lastPosition : Vector3.zero;
            }
        }

        void IUpdatable.OnUpdate(float duration)
        {
            prevPosition = lastPosition;
            lastPosition = Input.mousePosition;

            if (Input.GetMouseButtonDown((int) Button.Primary))
            {
                dragging = true;
                dragPosition = lastPosition;
            }
            else if (Input.GetMouseButtonUp((int) Button.Primary))
            {
                dragging = false;
            }
            else if (Input.GetMouseButton((int) Button.Primary))
            {
            }

            if (Input.GetMouseButtonDown((int) Button.Secondary))
            {
            }
            else if (Input.GetMouseButtonUp((int) Button.Secondary))
            {
            }
            else if (Input.GetMouseButton((int) Button.Secondary))
            {
            }
        }
    }
}
