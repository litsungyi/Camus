using UnityEngine;
using UnityEngine.EventSystems;

namespace Camus.Dragables
{
    public class DragTarget : MonoBehaviour, IDropHandler
    {
        private GameObject item;

        #region IDropHandler

        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            Debug.Log("OnDrop " + name);
            
            if(item == null)
            {
                DragSource.dragItem.transform.SetParent(transform);
            }
        }

        #endregion
    }
}
