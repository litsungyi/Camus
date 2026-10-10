using UnityEngine;
using UnityEngine.EventSystems;

namespace Camus.Draggables
{
    public abstract class DragSource : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public bool IsDragging
        {
            get;
            internal set;
        }

        public DropTarget Target
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
         * Called when dropped on a DropTarget holding another item.
         * Update the data of both items, then return how DragManager moves them.
         */
        public virtual MergeResult Merge(DragSource target) => MergeResult.Swapped;

        protected internal virtual void OnBeginDragging() {}

        protected internal virtual void OnEndDragging() {}

        // Drop failed, DragSource is moved back to origin
        protected internal virtual void OnCancelDragging() {}

        protected internal virtual void OnDropped(DropTarget dropTarget) {}
    }
}
