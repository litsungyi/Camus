using System.Collections.Generic;
using UnityEngine;

namespace Camus.Updatables
{
    internal class UpdateManagerHost : MonoBehaviour
    {
        internal enum Priority
        {
            VeryHigh,
            High,
            Middle,
            Low,
            VeryLow,
            MAX,
        }

        private IList<UpdateManager> updaters = new List<UpdateManager>((int) Priority.MAX);

        private void Awake()
        {
            for (var index = 0; index < (int) Priority.MAX; ++index)
            {
                updaters.Add(null);
            }
        }

        public void Add(Priority priority, UpdateManager manager)
        {
            var index = (int) priority;
            if (index > updaters.Count || updaters[index] != null)
            {
                Debug.LogWarning("Updater already exist.");
            }

            updaters[index] = manager;
        }

        private void OnFixedUpdate()
        {
            var duration = Time.fixedDeltaTime;
            for (var index = 0; index < updaters.Count; ++index)
            {
                var updater = updaters[index] as IFixedUpdatable;
                if (updater != null)
                {
                    updater.OnFixedUpdate(duration);
                }
            }
        }

        private void OnUpdate()
        {
            var duration = Time.deltaTime;
            for (var index = 0; index < updaters.Count; ++index)
            {
                var updater = updaters[index] as IUpdatable;
                if (updater != null)
                {
                    updater.OnUpdate(duration);
                }
            }
        }

        private void OnLateUpdate()
        {
            var duration = Time.deltaTime;
            for (var index = 0; index < updaters.Count; ++index)
            {
                var updater = updaters[index] as ILateUpdatable;
                if (updater != null)
                {
                    updater.OnLateUpdate(duration);
                }
            }
        }
    }
}
