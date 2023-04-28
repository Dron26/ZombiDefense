using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Service.DragAndDrop
{
    [DisallowMultipleComponent]
    public class DropObject : MonoCache,IDropHandler
    {
       [SerializeField] private DragAndDropController _controller;
    
        public void OnDrop(PointerEventData eventData) => 
            _controller.OnDrop(eventData,gameObject);

        public void SetController(DragAndDropController controller) => 
            _controller = controller;
    }
}
