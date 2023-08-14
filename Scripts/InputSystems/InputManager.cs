using System.Data;
using System.Collections.Generic;
using Camus.Updatables;
using Camus.EventSystems;
using Camus.Inputs.Events;
using UnityEngine;

namespace Camus.Inputs
{
    public class InputManager : MonoBehaviour, IUpdatable
    {
        public enum Button
        {
            Primary = 0, // often the left button
            Secondary = 1,
            Middle = 2,
            TouchBaseIndex = 10,
        }

        public IList<string> PredefineAxisKey = new List<string>()
        {
            "Horizontal",
            "Vertical",
            "Fire1",
            "Fire2",
            "Fire3",
            "Jump",
            "Mouse X",
            "Mouse Y",
            "Mouse ScrollWheel",
            "Submit",
            "Cancel",
        };

        private HashSet<string> registedInputKey = new HashSet<string>();

        private class ButtonStatus
        {
            public bool Pressed
            {
                get;
                private set;
            } = false;

            public float StartPressTime
            {
                get;
                private set;
            } = 0;

            public Vector3 StartPosition
            {
                get;
                private set;
            } = new Vector3(0, 0, 0);

            public Vector3 LastPosition
            {
                get;
                private set;
            } = new Vector3(0, 0, 0);

            public Vector3 CurrentPosition
            {
                get;
                private set;
            } = new Vector3(0, 0, 0);

            public Vector3 Distance
            {
                get => Pressed ?CurrentPosition - StartPosition : new Vector3(0, 0, 0);
            }

            public Vector3 Direction
            {
                get => Pressed ? CurrentPosition - LastPosition : new Vector3(0, 0, 0);
            }

            public void BeginPress(float time)
            {
                Pressed = true;
                StartPressTime = time;
                StartPosition = CurrentPosition;
            }

            public float EndPress(float time)
            {
                var startTime = StartPressTime;
                Pressed = false;
                StartPressTime = 0;
                return time - startTime;
            }

            public void TickPosition(Vector3 position)
            {
                CurrentPosition = LastPosition;
                LastPosition = position;
            }
        }

        private ButtonStatus mouseState = new ButtonStatus();

        public float LongPressThreshold
        {
            get;
            set;
        } = 0.5f;

        private Dictionary<Button, ButtonStatus> buttonStatuses = new Dictionary<Button, ButtonStatus>();

        private void Awake()
        {
            foreach (var axisKey in PredefineAxisKey)
            {
                registedInputKey.Add(axisKey);
            }
        }

        void IUpdatable.OnUpdate(float duration)
        {
            if (Input.mousePresent)
            {
                ProcessMouse(Button.Primary);
                ProcessMouse(Button.Secondary);
                ProcessMouse(Button.Middle);

                mouseState.TickPosition(Input.mousePosition);
                var movingEvent = new MovingEvent(Button.Primary, mouseState.CurrentPosition, mouseState.Direction, mouseState.Distance);
                EventDispatcher.Raise(movingEvent);
            }

            for (var count = 0; count < Input.touchCount; ++count)
            {
                ProcessTouch(count);
            }

            ProcessInput();
        }

        private void ProcessMouse(Button button)
        {
            var index = (int) button;
            if (!buttonStatuses.ContainsKey(button))
            {
                buttonStatuses.Add(button, new ButtonStatus());
            }

            var buttonStatus = buttonStatuses[button];
            buttonStatus.TickPosition(Input.mousePosition);

            if (Input.GetMouseButtonDown(index))
            {
                if (!buttonStatus.Pressed)
                {
                    buttonStatus.BeginPress(Time.time);
                    var beginPressEvent = new BeginPressEvent(button, buttonStatus.CurrentPosition);
                    EventDispatcher.Raise(beginPressEvent);
                }
            }
            else if (Input.GetMouseButtonUp(index))
            {
                if (buttonStatus.EndPress(Time.time) < LongPressThreshold)
                {
                    var pressEvent = new PressEvent(button, buttonStatus.CurrentPosition);
                    EventDispatcher.Raise(pressEvent);
                }
                else
                {
                    var longPressEvent = new LongPressEvent(button, buttonStatus.CurrentPosition);
                    EventDispatcher.Raise(longPressEvent);
                }


                var endPressEvent = new EndPressEvent(button, buttonStatus.CurrentPosition);
                EventDispatcher.Raise(endPressEvent);
            }
        }

        private void ProcessTouch(int index)
        {
            Touch touch = Input.GetTouch(index);
            Button button = (Button) (Button.TouchBaseIndex + index);
            if (!buttonStatuses.ContainsKey(button))
            {
                buttonStatuses.Add(button, new ButtonStatus());
            }
            var buttonStatus = buttonStatuses[button];
            buttonStatus.TickPosition(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    buttonStatus.BeginPress(Time.time);
                    var beginPressEvent = new BeginPressEvent(button, buttonStatus.CurrentPosition);
                    EventDispatcher.Raise(beginPressEvent);
                    break;

                case TouchPhase.Moved:
                    var movingEvent = new MovingEvent(button, buttonStatus.CurrentPosition, buttonStatus.Direction, buttonStatus.Distance);
                    EventDispatcher.Raise(movingEvent);
                    break;

                case TouchPhase.Ended:
                    if (buttonStatus.EndPress(Time.time) < LongPressThreshold)
                    {
                        var pressEvent = new PressEvent(button, buttonStatus.CurrentPosition);
                        EventDispatcher.Raise(pressEvent);
                    }
                    else
                    {
                        var longPressEvent = new LongPressEvent(button, buttonStatus.CurrentPosition);
                        EventDispatcher.Raise(longPressEvent);
                    }

                    var endPressEvent = new EndPressEvent(button, buttonStatus.CurrentPosition);
                    EventDispatcher.Raise(endPressEvent);
                    break;
            }
        }

        public void RegisterInput(string key)
        {
            if (!registedInputKey.Contains(key))
            {
                registedInputKey.Add(key);
            }
        }

        public void UnregisterInput(string key)
        {
            if (registedInputKey.Contains(key))
            {
                registedInputKey.Remove(key);
            }
        }

        private void ProcessInput()
        {
            var inputEvent = new InputEvent();
            foreach (var key in registedInputKey)
            {
                inputEvent.UpdateValue(key, Input.GetAxis(key));
            }

            EventDispatcher.Raise(inputEvent);
        }
    }
}
