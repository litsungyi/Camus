using UnityEngine;
using UnityEngine.EventSystems;

namespace Camus.Dragables
{
    public class DragTarget : MonoBehaviour, IDropHandler
    {
        public Transform dropPoint;
        public bool canDrop;

        #region IDropHandler

        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            if (canDrop)
            {
                DragManager.OnDrop(eventData.pointerDrag, this);
            }
        }

        #endregion
    }
}
