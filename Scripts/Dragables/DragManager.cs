using System.Collections.Generic;
using UnityEngine;

namespace Camus.Dragables
{
    public enum DraggableResult
    {
        Denied,
        Accept,
    }

    public enum DraggedResult
    {
        None,
        ResetPosition,
    }

    public enum DroppableResult
    {
        Denied,
        Accept,
    }

    public enum SourceDropResult
    {
        None,
        Destroy,
        ResetPosition,
        KeepPosition,
    }

    public enum TargetDropResult
    {
        Denied,
        Accept,
        Swap,
    }

    public enum DroppedResult
    {
        None,
        ResetPosition,
    }

    public static class DragManager
    {
        private class DragInfo
        {
            public DragSource Source;
            public Vector3 OriginPosition;
        }

        private static readonly Dictionary<GameObject, DragInfo> dragingObjects = new Dictionary<GameObject, DragInfo>();

        public static void BeginDrag(DragSource source)
        {
            if (source.IsDraggable() == DraggableResult.Accept)
            {
                AcceptBeginDrag();
            }
            else
            {
                DeniedBeginDrag();
            }

            void AcceptBeginDrag()
            {
                var dragInfo = new DragInfo
                {
                    Source = source,
                    OriginPosition = source.transform.position
                };

                dragingObjects.Add(source.gameObject, dragInfo);
            }
            
            void DeniedBeginDrag()
            {
                source.CancelDragging();
            }
        }

        public static void Dragging(DragSource source, Vector3 position)
        {
            source.transform.position = position;  // Input.mousePosition;
        }

        public static void EndDrag(DragSource source)
        {
            if (dragingObjects.TryGetValue(source.gameObject, out DragInfo dragInfo))
            {
                if (source.OnDragged() == DraggedResult.ResetPosition)
                {
                    ResetPosition(dragInfo);
                }

                dragingObjects.Remove(source.gameObject);
            }
        }

        public static void OnDrop(GameObject source, DropTarget target)
        {
            if (!dragingObjects.TryGetValue(source, out DragInfo dragInfo))
            {
                return;
            }

            if (target.IsDroppable(dragInfo.Source) == DroppableResult.Denied)
            {
                // NOTE: Cannot move 
                ResetPosition(dragInfo);
                return;
            }

            dragInfo.Source.OnDropped(target);
            dragingObjects.Remove(source.gameObject);
        }

        private static void ResetPosition(DragInfo dragInfo)
        {
            dragInfo.Source.transform.position = dragInfo.OriginPosition;
        }
    }
}
