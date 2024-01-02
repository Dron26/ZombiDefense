using Humanoids.AbstractLevel;
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

        private string _name;
        private int _health;
        private int _damage;
        private string _info;

        public void SetParametrs(Humanoid humanoid)
        {
            _name = humanoid.GetName();
            _health = humanoid.GetMaxHealth();
            WeaponController _weaponController = humanoid.GetComponent<WeaponController>();
            
            _damage = _weaponController.Damage;
            _info = humanoid.GetName()+"Info";

            ShowInfo();
        }

        public void ShowInfo()
        {
            _name= LeanLocalization.GetTranslationText(_name);
            _nameWindow.text = _name;
            _healthWindow.text = _health.ToString();
            _damageWindow.text = _damage.ToString();
            
            _infoWindow.text = LeanLocalization.GetTranslationText(_info);
        }
    }
}