using Humanoids.AbstractLevel;
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

        private string _name;
        private int _health;
        private int _damage;
        private string _info;

        public void SetParametrs(Humanoid humanoid)
        {
            _name = humanoid.GetName();
            _health = humanoid.GetMaxHealth();
            _damage = humanoid.GetWeaponController().GetWeapon().Damage;
            _info = humanoid.GetInfoName();

            ShowName();
        }

        public void ShowName()
        {
            _nameWindow.text = LeanLocalization.GetTranslationText(_name);
            _healthWindow.text = _health.ToString();
            _damageWindow.text = _damage.ToString();
            
            _infoWindow.text = LeanLocalization.GetTranslationText(_info);
        }
    }
}