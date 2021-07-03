using UnityEngine;
using UnityEngine.EventSystems;

namespace Camus.Dragables
{
    public abstract class DropTarget : MonoBehaviour, IDropHandler
    {
        public Transform dropPoint;

        [SerializeField]
        private bool droppable;

        public bool Droppable
        {
            get => droppable;
            set => droppable = value;
        }

        #region IDropHandler

        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            if (droppable)
            {
                DragManager.OnDrop(eventData.pointerDrag, this);
            }
        }

        #endregion

        // NOTE: Return DropResult.Accept if DragSource can drop on DropTarget
        //       Return DropResult.Denied if DragSource cannot drop on DropTarget
        public abstract DroppableResult IsDroppable(DragSource source);

        public abstract void OnDropped(DropTarget dropTarget);
    }
}
