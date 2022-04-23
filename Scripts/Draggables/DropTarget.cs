using UnityEngine;
using UnityEngine.EventSystems;

namespace Camus.Draggables
{
    public abstract class DropTarget : MonoBehaviour, IDropHandler
    {
        public bool IsDroppable
        {
            get;
            set;
        }

        #region IDropHandler

        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            if (!IsDroppable)
            {
                return;
            }
                
            DragManager.Drop(eventData.pointerDrag, this);
        }

        #endregion

        /**
         * Return DropResult.Accepted if DragSource can drop on DropTarget
         * Return DropResult.Denied if DragSource cannot drop on DropTarget
         */
        public virtual DroppableResult GetDroppableResult(DragSource source) => DroppableResult.Accepted;

        protected internal virtual void OnDropped(DragSource dragSource) {}
    }
}
