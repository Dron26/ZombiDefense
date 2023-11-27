using Humanoids.AbstractLevel;
using Humanoids.People;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    class AdditionalWeaponButton : MonoCache
    {
        [SerializeField] private Image _granade;
        [SerializeField] private Button _weapon;

        private Humanoid _humanoid;
        private GrenadeThrower _grenadeThrower;
        
        
        public void Initialize(SaveLoadService saveLoadService)
        {
            saveLoadService.OnSelectedNewHumanoid+=OnSelectedNewHumanoid;
            _weapon.onClick.AddListener(() =>TryGetGranade());
        }
       private void Start()
        {
            _weapon.interactable=false;
        }

    
       private void SwitchPanelState(bool state)
        {
            if (_humanoid.gameObject.TryGetComponent(out GrenadeThrower grenadeThrower))
            {
                _grenadeThrower=grenadeThrower;
                _weapon.interactable=state;
            }
            
            _weapon.interactable=state;
        }
    
        public void TryGetGranade()
        {
            _grenadeThrower.Throw();
        }

        private void OnSelectedNewHumanoid(Humanoid humanoid)
        {
            if (_humanoid==null & _humanoid != humanoid)
            {
                _humanoid=humanoid;
                _humanoid.GetComponent<WeaponController>().OnAddGranade+=() => SwitchPanelState(true);
                SwitchPanelState(false);
            }
        }
    
    
    }
}