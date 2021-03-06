﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Camus.Utilities;

namespace Camus.Dragables
{
    [RequireComponent(typeof(Image))]
    public abstract class DragSource : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        protected Image image;

        [ReadOnly, SerializeField]
        protected bool dragging;

        public bool IsDragging
        {
            get => dragging;
            set
            {
                image.raycastTarget = !value;
                dragging = value;
            }
        }

        public DropTarget Target
        {
            get;
            private set;
        } 

        #region IBeginDragHandler

        public void OnBeginDrag(PointerEventData eventData)
        {
            IsDragging = true;
            DragManager.BeginDrag(this);
        }

        #endregion

        #region IDragHandler

        public void OnDrag(PointerEventData eventData)
        {
            if (IsDragging)
            {
                DragManager.Dragging(this, eventData.position);
            }
        }

        #endregion

        #region IEndDragHandler

        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsDragging)
            {
                IsDragging = false;
                DragManager.EndDrag(this);
            }
        }

        #endregion

        // NOTE: Return DraggableResult.Accept if DragSource can begin drag
        //       Return DraggableResult.Denied if DragSource cannot begin drag
        public abstract DraggableResult IsDraggable();

        public abstract void CancelDragging();

        // NOTE: If return DraggedResult.None, keep DragSource at current position
        //       If return DraggedResult.ResetPosition, reset DragSource to origin position
        public abstract DraggedResult OnDragged();

        public abstract void OnDropped(DropTarget dropTarget);
    }
}
