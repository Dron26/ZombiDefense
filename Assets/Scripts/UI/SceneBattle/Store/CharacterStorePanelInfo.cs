using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Service.SaveLoadService;

namespace UI.SceneBattle.Store
{
    public class CharacterStorePanelInfo:MonoCache
    {
        private PlayerCharacterInitializer _characterInitializer;
        private SaveLoad _saveLoad;
        
        public void Initialize(PlayerCharacterInitializer characterInitializer, SaveLoad saveLoad)
        {
            _characterInitializer=characterInitializer;
            _saveLoad= saveLoad;
        }

        public void ShowInfo()
        {
            print("ShowInfo");
        }
    }
}