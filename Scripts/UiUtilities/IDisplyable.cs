using System;
using UnityEngine;

namespace Camus.UiUtilities
{
    public interface IDisplyable
    {
        void Show(Action<bool> callback = null);
        void Hide(bool destroy, Action<bool> callback = null);
        void Demolish();
        GameObject GetGameObject();

        void OnNotify(string state);
    }
}
