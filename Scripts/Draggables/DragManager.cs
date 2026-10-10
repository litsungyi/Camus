using System.Collections.Generic;
using Camus.EventSystems;
using Camus.Draggables.Events;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Camus.Draggables
{
    /**
     * uGUI calls OnDrop on the target before OnEndDrag on the source.
     * A successful Drop finishes the drag, so EndDrag only sees failed drops and moves the source back.
     */
    public static class DragManager
    {
        private class DragInfo
        {
            public Transform OriginParent;
            public int OriginSiblingIndex;
            public Vector3 OriginPosition;
        }

        private static readonly Dictionary<DragSource, DragInfo> draggingSources = new Dictionary<DragSource, DragInfo>();

        public static void BeginDrag(DragSource source, PointerEventData eventData)
        {
            if (source.GetDraggableResult() != DraggableResult.Accepted)
            {
                return;
            }

            var transform = source.transform;
            draggingSources[source] = new DragInfo
            {
                OriginParent = transform.parent,
                OriginSiblingIndex = transform.GetSiblingIndex(),
                OriginPosition = transform.position,
            };

            source.IsDragging = true;
            source.OnBeginDragging();
            EventDispatcher.Raise(new BeginDragEvent(source));
        }

        public static void EndDrag(DragSource source, PointerEventData eventData)
        {
            if (!draggingSources.TryGetValue(source, out DragInfo dragInfo))
            {
                return;
            }

            MoveToOrigin(source.transform, dragInfo);
            source.OnCancelDragging();
            FinishDrag(source);
        }

        public static void Dragging(DragSource source, PointerEventData eventData)
        {
            if (!draggingSources.ContainsKey(source))
            {
                return;
            }

            source.transform.position = eventData.position;
        }

        public static void Drop(GameObject sourceObject, DropTarget target)
        {
            if (sourceObject == null ||
                !sourceObject.TryGetComponent(out DragSource source) ||
                !draggingSources.TryGetValue(source, out DragInfo dragInfo))
            {
                return;
            }

            // Leave the drag running, EndDrag moves the source back to origin
            if (!target.IsDroppable || target.GetDroppableResult(source) != DroppableResult.Accepted)
            {
                return;
            }

            var other = target.Item;
            if (other == null || other == source)
            {
                Place(source, target);
            }
            else
            {
                Merge(source, other, target, dragInfo);
            }

            source.OnDropped(target);
            target.OnDropped(source);
            EventDispatcher.Raise(new DropEvent(source, target));
            FinishDrag(source);
        }

        private static void Merge(DragSource source, DragSource other, DropTarget target, DragInfo dragInfo)
        {
            switch (source.Merge(other))
            {
                case MergeResult.Stacked:
                case MergeResult.Combined:
                    Detach(source);
                    Object.Destroy(source.gameObject);
                    break;

                case MergeResult.Overflowed:
                    MoveToOrigin(source.transform, dragInfo);
                    break;

                case MergeResult.Swapped:
                default:
                    var originTarget = source.Target;
                    Place(source, target);
                    if (originTarget != null)
                    {
                        Place(other, originTarget);
                    }
                    else
                    {
                        other.Target = null;
                        MoveToOrigin(other.transform, dragInfo);
                    }
                    break;
            }
        }

        private static void Place(DragSource source, DropTarget target)
        {
            Detach(source);
            source.Target = target;
            target.Item = source;

            var point = target.DropPoint;
            source.transform.SetParent(point, true);
            source.transform.position = point.position;
        }

        private static void Detach(DragSource source)
        {
            if (source.Target != null && source.Target.Item == source)
            {
                source.Target.Item = null;
            }

            source.Target = null;
        }

        private static void MoveToOrigin(Transform transform, DragInfo dragInfo)
        {
            transform.SetParent(dragInfo.OriginParent, true);
            transform.SetSiblingIndex(dragInfo.OriginSiblingIndex);
            transform.position = dragInfo.OriginPosition;
        }

        private static void FinishDrag(DragSource source)
        {
            draggingSources.Remove(source);
            source.IsDragging = false;
            source.OnEndDragging();
            EventDispatcher.Raise(new EndDragEvent(source));
        }
    }
}
