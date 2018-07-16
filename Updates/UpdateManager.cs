using System.Collections.Generic;
using UnityEngine;

namespace Camus.Updates
{
    public class UpdateManager : MonoBehaviour
    {
        private HashSet<IUpdatable> updateEntities = new HashSet<IUpdatable>();
        private HashSet<IFixedUpdatable> fixedUpdateEntities = new HashSet<IFixedUpdatable>();
        private HashSet<ILateUpdatable> lateUpdateEntities = new HashSet<ILateUpdatable>();

        // NOTE: Only allow create UpdateManager from Camus
        internal UpdateManager()
        {
        }

        public void Register(MonoBehaviour entity)
        {
            var updatable = entity as IUpdatable;
            if (updatable != null)
            {
                updateEntities.Add(updatable);
            }

            var fixedUpdatable = entity as IFixedUpdatable;
            if (fixedUpdatable != null)
            {
                fixedUpdateEntities.Add(fixedUpdatable);
            }

            var lateUpdatable = entity as ILateUpdatable;
            if (lateUpdatable != null)
            {
                lateUpdateEntities.Add(lateUpdatable);
            }
        }

        public void Unregister(MonoBehaviour entity)
        {
            var updatable = entity as IUpdatable;
            if (updatable != null)
            {
                updateEntities.Remove(updatable);
            }

            var fixedUpdatable = entity as IFixedUpdatable;
            if (fixedUpdatable != null)
            {
                fixedUpdateEntities.Remove(fixedUpdatable);
            }

            var lateUpdatable = entity as ILateUpdatable;
            if (lateUpdatable != null)
            {
                lateUpdateEntities.Remove(lateUpdatable);
            }
        }

        private void Update()
        {
            var duration = Time.deltaTime;
            foreach (var item in updateEntities)
            {
                item.OnUpdate(duration);
            }
        }

        private void FixedUpdate()
        {
            var duration = Time.fixedDeltaTime;
            foreach (var item in fixedUpdateEntities)
            {
                item.OnFixedUpdate(duration);
            }
        }

        private void LateUpdate()
        {
            var duration = Time.deltaTime;
            foreach (var item in lateUpdateEntities)
            {
                item.OnLateUpdate(duration);
            }
        }
    }
}
