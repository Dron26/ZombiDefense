using Characters.Humanoids.AbstractLevel;
using Characters.Robots;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using Lean.Localization;
using TMPro;
using UnityEngine;

namespace UI.HUD.StorePanel
{
    public class CharacterInfoPanel : MonoCache
    {
        [SerializeField] private TextMeshProUGUI _nameWindow;
        [SerializeField] private TextMeshProUGUI _healthWindow;
        [SerializeField] private TextMeshProUGUI _damageWindow;
        [SerializeField] private TextMeshProUGUI _infoWindow;

        private CharacterStore _characterStore;
        private Character _character;

        public void SetParametrs()
        {
            gameObject.SetActive(true);
            _character = _characterStore.SelectedCharacter;
          //  _damageWindow.text = _character.GetComponent<IWeaponController>().Damage.ToString();
            _nameWindow.text = LeanLocalization.GetTranslationText(_character.GetType().ToString());
            _healthWindow.text = _character.Health.ToString();
            
            _infoWindow.text = LeanLocalization.GetTranslationText(_character.GetType() + "Info");
        }

        public void Initialize(CharacterStore characterStore)
        {
            _characterStore = characterStore;
            _characterStore.OnUpdateSelectedCharacter += SetParametrs;
        }
    }
}