using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Camus.Updatables
{
    public class UpdateManager : MonoBehaviour
    {
        private readonly HashSet<IUpdatable> updateEntities = new HashSet<IUpdatable>();
        private readonly HashSet<IFixedUpdatable> fixedUpdateEntities = new HashSet<IFixedUpdatable>();
        private readonly HashSet<ILateUpdatable> lateUpdateEntities = new HashSet<ILateUpdatable>();

        private readonly IList<object> registerList = new List<object>();
        private readonly IList<object> unregisterList = new List<object>();

        public void Register(MonoBehaviour entity)
        {
            registerList.Add(entity);
        }

        private void DoRegister()
        {
            if (!registerList.Any())
            {
                return;
            }

            foreach (var entity in registerList)
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

            registerList.Clear();
        }

        public void Unregister(MonoBehaviour entity)
        {
            unregisterList.Add(entity);
        }

        private void DoUnregister()
        {
            if (!unregisterList.Any())
            {
                return;
            }

            foreach (var entity in unregisterList)
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

            unregisterList.Clear();
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

            DoRegister();
            DoUnregister();
        }
    }
}
