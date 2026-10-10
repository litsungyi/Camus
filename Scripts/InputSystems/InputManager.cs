using System.Collections.Generic;
using Camus.Updatables;
using Camus.EventSystems;
using Camus.Inputs.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

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
                LastPosition = CurrentPosition;
                CurrentPosition = position;
            }
        }

        // Actions named after the legacy Input Manager axes, with its default bindings
        private readonly Dictionary<string, InputAction> predefinedActions = new Dictionary<string, InputAction>();

        private readonly Dictionary<string, InputAction> registedActions = new Dictionary<string, InputAction>();

        private ButtonStatus mouseState = new ButtonStatus();

        public float LongPressThreshold
        {
            get;
            set;
        } = 0.5f;

        public IEnumerable<string> PredefinedInputKeys => predefinedActions.Keys;

        private Dictionary<Button, ButtonStatus> buttonStatuses = new Dictionary<Button, ButtonStatus>();

        private void Awake()
        {
            AddPredefined(CreateAxis("Horizontal", "<Gamepad>/leftStick/x", ("<Keyboard>/a", "<Keyboard>/d"), ("<Keyboard>/leftArrow", "<Keyboard>/rightArrow")));
            AddPredefined(CreateAxis("Vertical", "<Gamepad>/leftStick/y", ("<Keyboard>/s", "<Keyboard>/w"), ("<Keyboard>/downArrow", "<Keyboard>/upArrow")));
            AddPredefined(CreateButton("Fire1", "<Keyboard>/leftCtrl", "<Mouse>/leftButton", "<Gamepad>/buttonSouth"));
            AddPredefined(CreateButton("Fire2", "<Keyboard>/leftAlt", "<Mouse>/rightButton", "<Gamepad>/buttonEast"));
            AddPredefined(CreateButton("Fire3", "<Keyboard>/leftShift", "<Mouse>/middleButton", "<Gamepad>/buttonWest"));
            AddPredefined(CreateButton("Jump", "<Keyboard>/space", "<Gamepad>/buttonNorth"));
            // Legacy mouse axes use sensitivity 0.1, scroll is 1 per notch with the default scrollDeltaBehavior
            AddPredefined(CreateValue("Mouse X", "<Mouse>/delta/x", "scale(factor=0.1)"));
            AddPredefined(CreateValue("Mouse Y", "<Mouse>/delta/y", "scale(factor=0.1)"));
            AddPredefined(CreateValue("Mouse ScrollWheel", "<Mouse>/scroll/y", "scale(factor=0.1)"));
            AddPredefined(CreateButton("Submit", "<Keyboard>/enter", "<Keyboard>/numpadEnter", "<Gamepad>/buttonSouth"));
            AddPredefined(CreateButton("Cancel", "<Keyboard>/escape", "<Gamepad>/buttonEast"));

            void AddPredefined(InputAction action)
            {
                predefinedActions.Add(action.name, action);
                RegisterInput(action.name, action);
            }
        }

        private void OnDestroy()
        {
            foreach (var action in predefinedActions.Values)
            {
                action.Dispose();
            }

            predefinedActions.Clear();
            registedActions.Clear();
        }

        void IUpdatable.OnUpdate(float duration)
        {
            var mouse = Mouse.current;
            if (mouse != null)
            {
                var position = (Vector3) mouse.position.ReadValue();
                ProcessButton(Button.Primary, mouse.leftButton, position);
                ProcessButton(Button.Secondary, mouse.rightButton, position);
                ProcessButton(Button.Middle, mouse.middleButton, position);

                mouseState.TickPosition(position);
                var movingEvent = new MovingEvent(Button.Primary, mouseState.CurrentPosition, mouseState.Direction, mouseState.Distance);
                EventDispatcher.Raise(movingEvent);
            }

            var touchscreen = Touchscreen.current;
            if (touchscreen != null)
            {
                var touches = touchscreen.touches;
                for (var index = 0; index < touches.Count; ++index)
                {
                    ProcessTouch(index, touches[index]);
                }
            }

            ProcessInput();
        }

        private ButtonStatus GetButtonStatus(Button button)
        {
            if (!buttonStatuses.TryGetValue(button, out var buttonStatus))
            {
                buttonStatus = new ButtonStatus();
                buttonStatuses.Add(button, buttonStatus);
            }

            return buttonStatus;
        }

        private void ProcessButton(Button button, ButtonControl control, Vector3 position)
        {
            var buttonStatus = GetButtonStatus(button);
            buttonStatus.TickPosition(position);

            if (control.wasPressedThisFrame)
            {
                BeginPress(button, buttonStatus);
            }
            else if (control.wasReleasedThisFrame)
            {
                EndPress(button, buttonStatus);
            }
        }

        private void ProcessTouch(int index, TouchControl touch)
        {
            var press = touch.press;
            if (!press.isPressed && !press.wasReleasedThisFrame)
            {
                return;
            }

            var button = (Button) (Button.TouchBaseIndex + index);
            var buttonStatus = GetButtonStatus(button);
            buttonStatus.TickPosition(touch.position.ReadValue());

            if (press.wasPressedThisFrame)
            {
                BeginPress(button, buttonStatus);
            }
            else if (press.wasReleasedThisFrame)
            {
                EndPress(button, buttonStatus);
            }
            else if (touch.delta.ReadValue() != Vector2.zero)
            {
                var movingEvent = new MovingEvent(button, buttonStatus.CurrentPosition, buttonStatus.Direction, buttonStatus.Distance);
                EventDispatcher.Raise(movingEvent);
            }
        }

        private void BeginPress(Button button, ButtonStatus buttonStatus)
        {
            if (buttonStatus.Pressed)
            {
                return;
            }

            buttonStatus.BeginPress(Time.time);
            var beginPressEvent = new BeginPressEvent(button, buttonStatus.CurrentPosition);
            EventDispatcher.Raise(beginPressEvent);
        }

        private void EndPress(Button button, ButtonStatus buttonStatus)
        {
            if (!buttonStatus.Pressed)
            {
                return;
            }

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

        /**
         * Register a predefined input key (the legacy axis names, such as "Horizontal" or "Jump")
         */
        public void RegisterInput(string key)
        {
            if (predefinedActions.TryGetValue(key, out var action))
            {
                RegisterInput(key, action);
            }
            else
            {
                Debug.LogWarning($"InputManager: \"{key}\" is not predefined, register it with an InputAction");
            }
        }

        /**
         * Register an action reported in InputEvent under key. The action is enabled while registered.
         */
        public void RegisterInput(string key, InputAction action)
        {
            UnregisterInput(key);
            registedActions.Add(key, action);
            action.Enable();
        }

        public void UnregisterInput(string key)
        {
            if (registedActions.TryGetValue(key, out var action))
            {
                registedActions.Remove(key);
                action.Disable();
            }
        }

        private void ProcessInput()
        {
            var inputEvent = new InputEvent();
            foreach (var pair in registedActions)
            {
                inputEvent.UpdateValue(pair.Key, pair.Value.ReadValue<float>());
            }

            EventDispatcher.Raise(inputEvent);
        }

        private static InputAction CreateAxis(string name, string stick, params (string negative, string positive)[] keys)
        {
            var action = new InputAction(name, InputActionType.Value, stick, expectedControlType: "Axis");
            foreach (var (negative, positive) in keys)
            {
                action.AddCompositeBinding("1DAxis")
                    .With("Negative", negative)
                    .With("Positive", positive);
            }

            return action;
        }

        private static InputAction CreateButton(string name, params string[] bindings)
        {
            var action = new InputAction(name, InputActionType.Button);
            foreach (var binding in bindings)
            {
                action.AddBinding(binding);
            }

            return action;
        }

        private static InputAction CreateValue(string name, string binding, string processors)
        {
            return new InputAction(name, InputActionType.Value, binding, processors: processors, expectedControlType: "Axis");
        }
    }
}
