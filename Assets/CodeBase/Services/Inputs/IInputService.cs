using UnityEngine.EventSystems;

namespace Services.Inputs
{
    public interface IInputService:IDragHandler, IBeginDragHandler, IEndDragHandler
    {}
}