using UnityEngine;
using UnityEngine.EventSystems;

namespace Camus.Dragables
{
    public class DragSource : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region IBeginDragHandler

        public void OnBeginDrag(PointerEventData eventData)
        {
            DragController.BeginDrag(this);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        #endregion

        #region IDragHandler

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        #endregion

        #region IEndDragHandler

       public void OnEndDrag(PointerEventData eventData)
        {
            DragController.EndDrag(this);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        #endregion
    }
}
