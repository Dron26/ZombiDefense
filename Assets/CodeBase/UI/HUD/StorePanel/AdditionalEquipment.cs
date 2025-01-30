using System;
using System.Collections;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Interface;
using Services;
using Services.SaveLoad;
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
        
        private Button _buttonAdditionalPanel;
        public event Action OnSelectedMedicineBox;
        public event Action OnSelectedWeaponBox;
        private GameEventBroadcaster _eventBroadcaster;

        private bool _isWeaponActive;
        private bool _isMedicineActive;
        private bool _isSelected;
        public void Initialize()
        {
            _eventBroadcaster=AllServices.Container.Single<GameEventBroadcaster>(); 

            AddListener();
            
            weaponBoxButton.SwitchStateButton(false);
            _medicineBoxButton.SwitchStateButton(false);
            _buttonAdditionalPanel=_additionalPanel.GetComponent<Button>();
            _buttonAdditionalPanel.onClick.AddListener(OnSelectButton);
        }
        private void AddListener()
        {
            _eventBroadcaster.OnSelectedNewPoint += CheckPointInfo;
            _medicineBoxButton.OnSelected+=SelectedMedicineBox;
            weaponBoxButton.OnSelected+=SelectedWeaponBox;
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
                StartCoroutine(StartTimer(3));
            }
            else
            {
                StartCoroutine(StartTimer(3));
            }
        }

        private IEnumerator StartTimer(int time)
        {
            yield return new WaitForSecondsRealtime(time);

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

        public void HideButton( )
        {
            StartCoroutine(StartTimer(0));
        }

        private void RemoveListener()
        {
            _eventBroadcaster.OnSelectedNewPoint -= CheckPointInfo;
            
            _medicineBoxButton.OnSelected-=SelectedMedicineBox;
            weaponBoxButton.OnSelected-=SelectedWeaponBox;
        }

        private void OnDestroy()
        {
            RemoveListener();
        }
    }
}