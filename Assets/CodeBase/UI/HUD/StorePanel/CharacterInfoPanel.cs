using Characters.Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
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
        private CharacterData _character;

        public void SetParametrs()
        {
            gameObject.SetActive(true);
            _character = _characterStore.SelectedCharacter;
            _damageWindow.text = _character.ItemData.Damage.ToString();
            _nameWindow.text = LeanLocalization.GetTranslationText(_character.Type.ToString());
            _healthWindow.text = _character.Health.ToString();
            
            _infoWindow.text = LeanLocalization.GetTranslationText(_character.Type.ToString() + "Info");
        }

        public void Initialize(CharacterStore characterStore)
        {
            _characterStore = characterStore;
            _characterStore.OnUpdateSelectedCharacter += SetParametrs;
            SetParametrs();
        }
    }
}