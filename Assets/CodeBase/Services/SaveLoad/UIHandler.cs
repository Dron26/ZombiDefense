using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Services.SaveLoad
{
    public class UIHandler:IUIHandler
    {
        private LoadingCurtain _curtain;
        private GraphicRaycaster _raycaster;
        private EventSystem _eventSystem;

        public void SetCurtain(LoadingCurtain curtain) => 
            _curtain = curtain;

        public LoadingCurtain GetCurtain() => _curtain;

        public void SetRaycaster(GraphicRaycaster raycaster) => 
            _raycaster = raycaster;
        public GraphicRaycaster GetRaycaster() => _raycaster;
        
        public void SetEvenSystem(EventSystem eventSystem)
        {
            _eventSystem=eventSystem;
        }
        public EventSystem GetEventSystem()=> _eventSystem;
    }
}