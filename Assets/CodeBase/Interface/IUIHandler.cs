using Services;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Interface
{
    public interface IUIHandler:IService
    {
        void SetCurtain(LoadingCurtain curtain);
        LoadingCurtain GetCurtain();
        void SetRaycaster(GraphicRaycaster raycaster);
        GraphicRaycaster GetRaycaster();
        EventSystem GetEventSystem();
        void SetEvenSystem(EventSystem eventSystem);
    }
}