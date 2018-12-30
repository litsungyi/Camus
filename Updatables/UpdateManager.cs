using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Camus.Updatables
{
    public class UpdateManager : IFixedUpdatable, IUpdatable, ILateUpdatable
    {
        private enum RegisterType
        {
            Add,
            Remove,
        }

        private class RegisterInfo
        {
            public MonoBehaviour entity;
            public RegisterType type;
        }

        private readonly HashSet<IFixedUpdatable> fixedUpdateEntities = new HashSet<IFixedUpdatable>();
        private readonly HashSet<IUpdatable> updateEntities = new HashSet<IUpdatable>();
        private readonly HashSet<ILateUpdatable> lateUpdateEntities = new HashSet<ILateUpdatable>();

        private readonly IList<RegisterInfo> registerList = new List<RegisterInfo>();

        public void Register(MonoBehaviour entity)
        {
            registerList.Add(new RegisterInfo()
            {
                entity = entity,
                type = RegisterType.Add
            });
        }

        public void Unregister(MonoBehaviour entity)
        {
            registerList.Add(new RegisterInfo()
            {
                entity = entity,
                type = RegisterType.Remove
            });
        }

        private void UpdateRegisterList()
        {
            if (!registerList.Any())
            {
                return;
            }

            foreach (var info in registerList)
            {
                var fixedUpdatable = info.entity as IFixedUpdatable;
                if (fixedUpdatable != null)
                {
                    if (info.type == RegisterType.Add)
                    {
                        fixedUpdateEntities.Add(fixedUpdatable);
                    }
                    else
                    {
                        fixedUpdateEntities.Remove(fixedUpdatable);
                    }
                }

                var updatable = info.entity as IUpdatable;
                if (updatable != null)
                {
                    if (info.type == RegisterType.Add)
                    {
                        updateEntities.Add(updatable);
                    }
                    else
                    {
                        updateEntities.Remove(updatable);
                    }
                }

                var lateUpdatable = info.entity as ILateUpdatable;
                if (lateUpdatable != null)
                {
                    if (info.type == RegisterType.Add)
                    {
                        lateUpdateEntities.Add(lateUpdatable);
                    }
                    else
                    {
                        lateUpdateEntities.Remove(lateUpdatable);
                    }
                }
            }

            registerList.Clear();
        }

        void IFixedUpdatable.OnFixedUpdate(float duration)
        {
            foreach (var item in fixedUpdateEntities)
            {
                item.OnFixedUpdate(duration);
            }

            UpdateRegisterList();
        }

        void IUpdatable.OnUpdate(float duration)
        {
            foreach (var item in updateEntities)
            {
                item.OnUpdate(duration);
            }

            UpdateRegisterList();
        }

        void ILateUpdatable.OnLateUpdate(float duration)
        {
            foreach (var item in lateUpdateEntities)
            {
                item.OnLateUpdate(duration);
            }

            UpdateRegisterList();
        }
    }
}
