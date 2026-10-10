using UnityEngine;
using UnityEngine.EventSystems;

namespace Camus.Draggables
{
    public abstract class DropTarget : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Transform dropPoint;

        public bool IsDroppable
        {
            get;
            set;
        }

        public DragSource Item
        {
            get;
            internal set;
        }

        // Not ??: an unassigned serialized field is only null to Unity's == operator
        public Transform DropPoint => dropPoint != null ? dropPoint : transform;

        #region IDropHandler

        void IDropHandler.OnDrop(PointerEventData eventData)
        {
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
