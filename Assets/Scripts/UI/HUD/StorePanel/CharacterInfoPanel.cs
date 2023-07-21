using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Humanoids.People;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Lean.Localization;
using TMPro;
using UnityEngine;

namespace UI.HUD.Store
{
    public class CharacterInfoPanel:MonoCache
    {
        [SerializeField] private List<string> character;
        [SerializeField] private LeanLocalizedTextMeshProUGUI characterSkinnedMeshes;
        [SerializeField] private TextMeshProUGUI _nameWindow;
        [SerializeField] private TextMeshProUGUI _healthWindow;
        [SerializeField] private TextMeshProUGUI _damageWindow;
        [SerializeField] private TextMeshProUGUI _infoWindow;

        private string _name;
        private int _health;
        private int _damage;
        private string _info;
        
        string name;
            public void SetParametrs(Humanoid humanoid)
        {
            _name=humanoid.GetName();
            _health=humanoid.GetHealth();
            _damage = humanoid.GetWeaponController().GetDamage();
            _info = humanoid.GetInfoName();
            
            ShowName();
        }

            public void ShowName()
            {
               
                _nameWindow.text = LeanLocalization.GetTranslationText(name);
                _healthWindow.text = _health.ToString(); 
                _damageWindow.text = _damage.ToString();; 
                _infoWindow.text = LeanLocalization.GetTranslationText(_info);
            }
    }
}