using Service.DragAndDrop;
using UnityEngine.EventSystems;

namespace Service.Inputs
{
    public interface IInputService:IDragHandler, IBeginDragHandler, IEndDragHandler
    {}
}