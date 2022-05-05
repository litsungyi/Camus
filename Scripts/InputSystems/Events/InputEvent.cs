using System.Collections.Generic;
using Camus.EventSystems;
using UnityEngine;

namespace Camus.Inputs.Events
{
    public class InputEvent : IDomainEvent
    {
        public InputEvent()
        {
        }

        public void UpdateValue(string key, float value)
        {
            Values[key] = value;
        }

        public IDictionary<string, float> Values
        {
            get;
            private set;
        } = new Dictionary<string, float>();

        public bool IsHigh(string key)
        {
            var value = 0f;
            if (Values.TryGetValue(key, out value))
            {
                return value >= 0.9999999f || value <= -0.9999999f;
            }

            return false;
        }

        public bool IsLow(string key)
        {
            var value = 0f;
            if (Values.TryGetValue(key, out value))
            {
                return value <= 0.0000001f && value >= -0.0000001f;
            }

            return false;
        }

        public bool IsPositive(string key)
        {
            var value = 0f;
            if (Values.TryGetValue(key, out value))
            {
                return value >= 0.0000001f;
            }

            return false;
        }

        public bool IsNegative(string key)
        {
            var value = 0f;
            if (Values.TryGetValue(key, out value))
            {
                return value <= -0.0000001f;
            }

            return false;
        }

        public float GetValue(string key)
        {
            var value = 0f;
            Values.TryGetValue(key, out value);
            return value;
        }
    }
}
