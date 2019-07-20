using System;
using UnityEngine;

namespace Camus.UiUtilities
{
    public class UiBaseController : MonoBehaviour, IDisplyable
    {
        #region IDisplyable

        public virtual void Show(Action<bool> callback)
        {
            UiManager.Instance.ShowUi(name, callback);
        }

        public virtual void Hide(bool destroy, Action<bool> callback)
        {
            if (destroy)
            {
                UiManager.Instance.DestroyUi(name, callback);
            }
            else
            {
                UiManager.Instance.HideUi(name, callback);
            }
        }

        #endregion
    }
}
