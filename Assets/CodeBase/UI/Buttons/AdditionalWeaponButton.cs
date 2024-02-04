using System;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.AIBattle;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class AdditionalWeaponButton : MonoCache
    {
        [SerializeField] private Image _granade;
        [SerializeField] private Button _weapon;

        private Humanoid _humanoid;
        private GrenadeThrower _grenadeThrower;
        private WeaponController _weaponController;
        public Action OnClickButton;

        public void Initialize(SaveLoadService saveLoadService)
        {
            saveLoadService.OnSelectedNewHumanoid+=OnSelectedNewHumanoid;
            _weapon.onClick.AddListener(() =>TryThrowGranade());
        }
       private void Start()
        {
            _weapon.interactable=false;
        }

    
       private void SwitchPanelState()
        {
            if (_weaponController.IsCanThrowGranade)
            {
                _grenadeThrower = _humanoid.gameObject.GetComponent<GrenadeThrower>();
                _weapon.interactable=true;
            }
            else
            {
                _grenadeThrower = null;
                _weapon.interactable=false;
            }
        }

       
    
        public void TryThrowGranade()
        {
            _weapon.interactable=false;
            OnClickButton?.Invoke();
        }

        private void OnSelectedNewHumanoid(Humanoid humanoid)
        {
            if (_humanoid==null & _humanoid != humanoid)
            {
                _humanoid=humanoid;
                _weaponController = _humanoid.GetComponent<WeaponController>();
                _weaponController.OnChangeGranade+=() => SwitchPanelState();
                _weaponController.SetAdditionalWeaponButton(this);
                SwitchPanelState();
            }
        }
    
    
    }
}