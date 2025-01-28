using Interface;
using UnityEngine.UI;

namespace Services.SaveLoad
{
    public class UIHandler:IUIHandler
    {
        private LoadingCurtain _curtain;
        private GraphicRaycaster _raycaster;

        public void SetCurtain(LoadingCurtain curtain) => 
            _curtain = curtain;

        public LoadingCurtain GetCurtain() => _curtain;

        public void SetRaycaster(GraphicRaycaster raycaster) => 
            _raycaster = raycaster;

        public GraphicRaycaster GetRaycaster() => _raycaster;
    }
}