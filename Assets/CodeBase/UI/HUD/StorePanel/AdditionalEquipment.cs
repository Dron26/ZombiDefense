using System;
using System.Collections;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Service.SaveLoad;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.StorePanel
{
    public  class AdditionalEquipment:MonoCache
    {
        [SerializeField]  private MedicineBoxButton _medicineBoxButton;
        [SerializeField]  private WeaponBoxButton weaponBoxButton;
        [SerializeField] private GameObject _additionalPanel;
        [SerializeField] private GameObject _additionalPanelButton;
        private Button _button;
        public event Action OnSelectedMedicineBox;
        public event Action OnSelectedWeaponBox;

        private SaveLoadService _saveLoadService;
        private bool _isWeaponActive;
        private bool _isMedicineActive;
        private bool _isSelected;
        public void Initialize(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            
            _saveLoadService.OnSelectedNewPoint += CheckPointInfo;
            
            _medicineBoxButton.OnSelected+=SelectedMedicineBox;
            weaponBoxButton.OnSelected+=SelectedWeaponBox;
            
            weaponBoxButton.SwitchStateButton(false);
                
            _medicineBoxButton.SwitchStateButton(false);
            _button=_additionalPanel.GetComponent<Button>();
            _button.onClick.AddListener(OnSelectButton);
        }

        private void SelectedMedicineBox()
        {
            OnSelectedMedicineBox?.Invoke();
        }
        
        private void SelectedWeaponBox()
        {
            OnSelectedWeaponBox?.Invoke();
        }
        
        private void OnSelectButton()
        {
            _isSelected = !_additionalPanelButton.activeSelf;
            
            _additionalPanelButton.SetActive(_isSelected);
            
            if (_isSelected)
            {
                StartCoroutine(StartTimer());
            }
            else
            {
                StartCoroutine(StartTimer());
            }
        }

        private IEnumerator StartTimer()
        {
            yield return new WaitForSecondsRealtime(3);

            _isSelected = false;
            _additionalPanelButton.SetActive(false);
        }
        
        private void CheckPointInfo(WorkPoint workPoint)
        {
            _isWeaponActive=!workPoint.IsHaveWeaponBox;
            _isMedicineActive=!workPoint.IsHaveMedicineBox;
 
            weaponBoxButton.SwitchStateButton(_isWeaponActive);
                
            _medicineBoxButton.SwitchStateButton(_isMedicineActive);

            if (_isMedicineActive==_isWeaponActive==false)
            {
                _additionalPanelButton.SetActive(_isMedicineActive);
            }
        }

        public void SwitchStateButton(bool isActive)
        {
            _additionalPanel.SetActive(isActive);
        }
    }
}