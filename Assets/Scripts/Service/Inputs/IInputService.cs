using Service.DragAndDrop;
using UnityEngine.EventSystems;

namespace Service.Inputs
{
    public interface IInputService:IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData);

        public void OnDrag(PointerEventData eventData);

        public void OnEndDrag(PointerEventData eventData);

        public void SetController(DragAndDropController controller);
        }
}