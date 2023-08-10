using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.Inputs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Service.DragAndDrop
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    
    public class DragObject : MonoCache,IInputService
    {
        private DragAndDropController _controller;
        private CanvasGroup _canvasGroup;

        private void Awake() => 
            _canvasGroup = GetComponent<CanvasGroup>();

        public void OnBeginDrag(PointerEventData eventData) => 
            _controller.OnBeginDrag(eventData,gameObject);

        public void OnDrag(PointerEventData eventData) => 
            _controller.OnDrag(eventData,gameObject);

        public void OnEndDrag(PointerEventData eventData) => 
            _controller.OnEndDrag(gameObject);

        public void SetController(DragAndDropController controller) => 
            _controller = controller;
    }
}