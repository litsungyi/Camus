using UnityEngine;
using UnityEngine.EventSystems;

namespace Camus.Dragables
{
    public class DragSource : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static GameObject dragItem;
        private Vector3 startPosition;

        #region IBeginDragHandler

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag");
            dragItem = gameObject;
            startPosition = transform.position;
            //GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        #endregion

        #region IDragHandler

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("OnDrag");
            transform.position = Input.mousePosition;
        }

        #endregion

        #region IEndDragHandler

       public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("OnEndDrag");

            dragItem = null;
            //transform.position = startPosition;
        }

        #endregion
    }
}
