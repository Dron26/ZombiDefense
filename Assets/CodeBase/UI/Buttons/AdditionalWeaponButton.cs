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

        private Character _character;
        private GrenadeThrower _grenadeThrower;
        private HumanoidWeaponController _humanoidWeaponController;
        public Action OnClickButton;
private bool _haveAdditionalWeapon;
        public void Initialize(SaveLoadService saveLoadService)
        {
            saveLoadService.OnSelectedNewCharacter+=OnSelectedNewCharacter;
            _weapon.onClick.AddListener(() =>TryThrowGranade());
        }
       private void Start()
        {
            _weapon.interactable=false;
        }

    
       private void SwitchPanelState()
        {
            if (_haveAdditionalWeapon)
            {
                if (_humanoidWeaponController.IsCanThrowGranade)
                {
                    _grenadeThrower = _character.gameObject.GetComponent<GrenadeThrower>();
                    _weapon.interactable=true;
                }
                else
                {
                    _grenadeThrower = null;
                    _weapon.interactable=false;
                }
            }
            else
            {
                _weapon.interactable=false;
            }
        }
  
        public void TryThrowGranade()
        {
            _weapon.interactable=false;
            OnClickButton?.Invoke();
        }

        private void OnSelectedNewCharacter(Character character)
        {
            if (_character==null & _character != character)
            {
                _character=character;
                
                if (_character.TryGetComponent(out HumanoidWeaponController weaponController ))
                {
                    _humanoidWeaponController=weaponController;
                    _humanoidWeaponController.OnChangeGranade+=() => SwitchPanelState();
                    _humanoidWeaponController.SetAdditionalWeaponButton(this);
                    _haveAdditionalWeapon=true;
                    SwitchPanelState();
                }
                else
                {
                        
                }
            }
        }
    
    
    }
}