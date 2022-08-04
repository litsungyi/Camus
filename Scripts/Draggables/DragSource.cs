using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Camus.Utilities;

namespace Camus.Draggables
{
    public abstract class DragSource : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public bool IsDragging
        {
            get;
            internal set;
        }

        #region IBeginDragHandler

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (IsDragging)
            {
                return;
            }

            DragManager.BeginDrag(this, eventData);
        }

        #endregion

        #region IEndDragHandler

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (!IsDragging)
            {
                return;
            }

            DragManager.EndDrag(this, eventData);
        }

        #endregion

        #region IDragHandler

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (!IsDragging)
            {
                return;
            }

            DragManager.Dragging(this, eventData);
        }

        #endregion

        /**
         * Return DraggableResult.Accepted if DragSource can begin drag
         * Return DraggableResult.Denied if DragSource cannot begin drag
         */
        public virtual DraggableResult GetDraggableResult() => DraggableResult.Accepted;

        /**
         * If return DraggedResult.None, keep DragSource at current position
         * If return DraggedResult.ResetPosition, reset DragSource to origin position
         */
        public virtual DraggedResult GetDraggedResult() => DraggedResult.None;

        protected internal virtual void OnBeginDragging() {}

        protected internal virtual void OnEndDragging() {}

        protected internal virtual void OnCancelDragging() {}

        protected internal virtual void OnDropped(DropTarget dropTarget) {}
    }
}
