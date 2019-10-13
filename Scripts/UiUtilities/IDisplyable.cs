using System;

namespace Camus.UiUtilities
{
    public interface IDisplyable
    {
        void Show(Action<bool> callback = null);
        void Hide(bool destroy, Action<bool> callback = null);
        void Demolish();
    }
}
