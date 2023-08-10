using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Service.SaveLoadService;
using UnityEngine.UI;

namespace UI.SceneBattle.Store
{
    public class WorkPointUpgradePanel:MonoCache
    {
        private PlayerCharacterInitializer _characterInitializer;
        private SaveLoadService _saveLoadService;
        private Button _button;
        
        public void Initialize(PlayerCharacterInitializer characterInitializer, SaveLoadService saveLoadService)
        {
            _characterInitializer=characterInitializer;
            _saveLoadService= saveLoadService;
            _button=GetComponentInChildren<Button>();
            _button.gameObject.SetActive(true);
        }

        public void ShowInfo()
        {
            print("ShowInfo");
        }

        public void SwitchStateButton(bool isActive)
        {
            _button.gameObject.SetActive(isActive);
        }

        public Button GetButton() => _button.gameObject.GetComponent<Button>();
    }
}