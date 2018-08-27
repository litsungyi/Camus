using System.Collections.Generic;
using UnityEngine;

namespace Camus.Dragables
{
    public class DragController
    {
        private class DragInfo
        {
            public DragSource source;
            public Vector3 originPosition;
        }

        private static Dictionary<GameObject, DragInfo> dragingObjects = new Dictionary<GameObject, DragInfo>();

        public static void BeginDrag(DragSource source)
        {
            var dragInfo = new DragInfo()
            {
                source = source,
                originPosition = source.transform.position
            };

            dragingObjects.Add(source.gameObject, dragInfo);
        }

        public static void OnDrop(GameObject source, DragTarget target)
        {
            source.transform.SetParent(target.transform);
            source.transform.localPosition = target.dropPoint.localPosition;
            dragingObjects.Remove(source);
        }

        public static void EndDrag(DragSource source)
        {
            DragInfo info;
            if(dragingObjects.TryGetValue(source.gameObject, out info))
            {
                source.transform.position = info.originPosition;
                dragingObjects.Remove(source.gameObject);
            }
        }
	}
}
