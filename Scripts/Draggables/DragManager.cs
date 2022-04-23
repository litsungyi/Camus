using System.Collections.Generic;
using Camus.EventSystems;
using Camus.Draggables.Events;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Camus.Draggables
{
    public static class DragManager
    {
        private class DragInfo
        {
            public DragSource Source;
            public Vector3 OriginPosition;
        }

        private static readonly Dictionary<GameObject, DragInfo> dragingObjects = new Dictionary<GameObject, DragInfo>();

        public static void BeginDrag(DragSource source, PointerEventData eventData)
        {
            switch (source.GetDraggableResult())
            {
                case DraggableResult.Accepted:
                    AcceptBeginDrag();
                    break;

                case DraggableResult.Denied:
                default:
                    DeniedBeginDrag();
                    break;
            }

            void AcceptBeginDrag()
            {
                var dragInfo = new DragInfo
                {
                    Source = source,
                    OriginPosition = source.transform.position
                };
                dragingObjects.Add(source.gameObject, dragInfo);

                source.IsDragging = true;
                source.OnBeginDragging();
                EventDispatcher.Raise(new BeginDragEvent(source));
            }

            void DeniedBeginDrag()
            {
                source.IsDragging = false;
                source.OnCancelDragging();
            }
        }

        public static void EndDrag(DragSource source, PointerEventData eventData)
        {
            if (!source.IsDragging || !dragingObjects.TryGetValue(source.gameObject, out DragInfo dragInfo))
            {
                return;
            }

            switch (source.GetDraggedResult())
            {
                case DraggedResult.ResetPosition:
                    ResetPosition(dragInfo);
                    break;

                case DraggedResult.None:
                default:
                    break;
            }

            dragingObjects.Remove(source.gameObject);

            source.IsDragging = false;
            source.OnEndDragging();
            EventDispatcher.Raise(new EndDragEvent(source));
        }

        public static void Dragging(DragSource source, PointerEventData eventData)
        {
            if (!source.IsDragging || !dragingObjects.TryGetValue(source.gameObject, out DragInfo dragInfo))
            {
                return;
            }

            source.transform.position = eventData.position;
        }

        public static void Drop(GameObject sourceObject, DropTarget target)
        {
            if (!target.IsDroppable)
            {
                return;
            }

            if (!dragingObjects.TryGetValue(sourceObject, out DragInfo dragInfo))
            {
                return;
            }

            switch (target.GetDroppableResult(dragInfo.Source))
            {
                case DroppableResult.Accepted:
                    AcceptDrop();
                    break;

                case DroppableResult.Denied:
                default:
                    break;
            }

            void AcceptDrop()
            {
                dragingObjects.Remove(sourceObject);

                dragInfo.Source.IsDragging = false;
                dragInfo.Source.OnDropped(target);
                target.OnDropped(dragInfo.Source);
                EventDispatcher.Raise(new DropEvent(dragInfo.Source, target));
            }
        }

        private static void ResetPosition(DragInfo dragInfo)
        {
            dragInfo.Source.transform.position = dragInfo.OriginPosition;
        }
    }
}
