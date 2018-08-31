using UnityEngine;
using UnityEngine.EventSystems;

namespace Camus.Dragables
{
    public class DragSource : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region IBeginDragHandler

        public void OnBeginDrag(PointerEventData eventData)
        {
            DragManager.BeginDrag(this);
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
            DragManager.EndDrag(this);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        #endregion
    }
}
