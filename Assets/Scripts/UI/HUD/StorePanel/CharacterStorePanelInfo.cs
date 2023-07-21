using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Service.SaveLoadService;
using UnityEngine.UI;

namespace UI.SceneBattle.Store
{
    public class CharacterStorePanelInfo:MonoCache
    {
        private PlayerCharacterInitializer _characterInitializer;
        private SaveLoad _saveLoad;
        private Button _button;
        
        public void Initialize(PlayerCharacterInitializer characterInitializer, SaveLoad saveLoad)
        {
            _characterInitializer=characterInitializer;
            _saveLoad= saveLoad;
            _button=GetComponentInChildren<Button>();
            _button.gameObject.SetActive(true);
        }

        public void ShowInfo()
        {
            print("ShowInfo");
        }

        public void ShowButton(bool isActive)
        {
            _button.gameObject.SetActive(isActive);
        }

        public Button GetButton() => _button.gameObject.GetComponent<Button>();
    }
}