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

        private void FixedUpdate()
        {
            var duration = Time.fixedDeltaTime;
            for (var index = 0; index < updaters.Count; ++index)
            {
                if (updaters[index] is IFixedUpdatable updater)
                {
                    updater.OnFixedUpdate(duration);
                }
            }
        }

        private void Update()
        {
            var duration = Time.deltaTime;
            for (var index = 0; index < updaters.Count; ++index)
            {
                if (updaters[index] is IUpdatable updater)
                {
                    updater.OnUpdate(duration);
                }
            }
        }

        private void LateUpdate()
        {
            var duration = Time.deltaTime;
            for (var index = 0; index < updaters.Count; ++index)
            {
                if (updaters[index] is ILateUpdatable updater)
                {
                    updater.OnLateUpdate(duration);
                }
            }
        }
    }
}
