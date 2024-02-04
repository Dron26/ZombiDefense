using Characters.Humanoids.AbstractLevel;
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



        public void SetParametrs(Humanoid humanoid)
        {
     
               
                _nameWindow.text = LeanLocalization.GetTranslationText(humanoid.GetName());
                _healthWindow.text = humanoid.GetMaxHealth().ToString();
                _damageWindow.text = humanoid.GetComponent<WeaponController>().Damage.ToString();
            
                _infoWindow.text = LeanLocalization.GetTranslationText(humanoid.GetName()+"Info");
            
        }
    }
}