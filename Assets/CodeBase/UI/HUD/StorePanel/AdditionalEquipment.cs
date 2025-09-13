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
        [SerializeField]  private Button _medicineBoxButton;
        [SerializeField]  private Button _weaponBoxButton;
        [SerializeField] private GameObject _additionalPanel;
        [SerializeField] private GameObject _additionalPanelButton;
        
        private Button _buttonAdditionalPanel;
        public event Action OnSelectedMedicineBox;
        public event Action OnSelectedWeaponBox;
        private IGameEventBroadcaster _eventBroadcaster;

        private bool _isWeaponActive;
        private bool _isMedicineActive;
        private bool _isSelected;
        public void Initialize()
        {
            _eventBroadcaster=AllServices.Container.Single<IGameEventBroadcaster>(); 

            AddListener();
            
            _weaponBoxButton.gameObject.SetActive(false);
            _medicineBoxButton.gameObject.SetActive(false);
            _buttonAdditionalPanel=_additionalPanel.GetComponent<Button>();
            _buttonAdditionalPanel.onClick.AddListener(OnSelectButton);
        }
        private void AddListener()
        {
            _eventBroadcaster.OnSelectedNewPoint += CheckPointInfo;
            _medicineBoxButton.onClick.AddListener(SelectedMedicineBox);
            _weaponBoxButton.onClick.AddListener(SelectedWeaponBox);
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
                StopCoroutine(StartTimer(3));
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
 
            _weaponBoxButton.gameObject.SetActive(_isWeaponActive);
            
            _medicineBoxButton.gameObject.SetActive(_isMedicineActive);

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
            
            _medicineBoxButton.onClick.RemoveListener(SelectedMedicineBox);
            _weaponBoxButton.onClick.RemoveListener(SelectedWeaponBox);
        }

        private void OnDestroy()
        {
            RemoveListener();
        }
    }
}