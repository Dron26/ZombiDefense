using System;
using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.Inits;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Infrastructure.Factories.FactoriesBox;

namespace UI.HUD.StorePanel
{
    public class Store : MonoCache
    {
        [SerializeField] private GameObject _storePanel;
        
        [SerializeField] private CharacterStore _characterStore;
        [SerializeField] private CharacterStore _eliteCharacterStore;
        [SerializeField] private GameObject _buttonPanel;
        [SerializeField] private AdsStore _adsStore;
        [SerializeField] private GameObject _applyAdsMoneyWindow;
        
        [SerializeField] CharacterStoreRotation _characterStoreRotation;
        
        [SerializeField] private Button _buttonStorePanel;
        [SerializeField] private Button _buttonRightPanel;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _applyAdsMoneyWindowButton;
        [SerializeField] private Button _closeAdsMoneyWindowButton;
        
        [SerializeField] private GameObject _controlPanel;
        [SerializeField] private GameObject _rightButtonPanel;
        [SerializeField] private WorkPointUpgradePanel _pointUpgradePanel;
         private WorkPointGroup _workPointGroup;
        [SerializeField] private int _priceForWorkPointUp;
       
        [SerializeField] private AdditionalEquipment _additionalEquipmentButton;
        
        [SerializeField] private Image _dimImage;
        [SerializeField] private Camera _cameraPhysical;
        [SerializeField] private Camera _cameraUI;
        [SerializeField] private Camera _characterVisual;

        
        private bool isButtonPanelOpen = true;
        private WorkPoint _selectedWorkPoint;
        private List<Humanoid> _characters = new();
        private SceneInitializer _sceneInitializer;
        private PlayerCharacterInitializer _characterInitializer;
        private MovePointController _movePointController;
        private SaveLoadService _saveLoadService;
        private int maxLevel = 3;
        private bool _isPanelActive=false;

        public UnityAction<bool> IsStoreActive;
        public Action<WorkPoint> OnBoughtUpgrade;
        private TimeManager _timeManager;

        private int _medicineBoxPrice = 100;
        private int _weaponBoxPrice = 200;
        
   
        private BoxFactory _boxFactory;
        
        public void Initialize(SceneInitializer initializer, SaveLoadService saveLoadService, TimeManager timeManager)
        {
            _timeManager = timeManager;
            _storePanel.gameObject.SetActive(!_storePanel.activeSelf);
            _saveLoadService = saveLoadService;
            _sceneInitializer = initializer;
            _workPointGroup = _sceneInitializer.GetPlayerCharacterInitializer().GetWorkPointGroup();
            SetCharacterInitializer();
            _storePanel.gameObject.SetActive(!_storePanel.activeSelf);
            _adsStore.Initialize(_saveLoadService);
            _additionalEquipmentButton.Initialize(_saveLoadService);

            _boxFactory=GetComponent<BoxFactory>();
        }

        private void SetCharacterInitializer()
        {
            _characterInitializer = _sceneInitializer.GetPlayerCharacterInitializer();
            //_characterInitializer.OnClickWorkpoint += CheckPointInfo;
            _characters = _saveLoadService.GetAvailableCharacters();
            _saveLoadService.OnSelectedNewPoint += CheckPointInfo;
            
            _characterStore.Initialize(_saveLoadService,this);
            _characterStore.OnCharacterBought += OnCharacterBought;
            _characterStore.OnMoneyEmpty += ShowPanelAdsForMoney;
            
            _additionalEquipmentButton.OnSelectedMedicineBox+=OnSelectMedicineBox;
            _additionalEquipmentButton.OnSelectedWeaponBox+=OnSelectWeaponBox;
            
            _eliteCharacterStore.Initialize(_saveLoadService,this);
            _eliteCharacterStore.OnCharacterBought += OnCharacterBought;
            
            InitializeButton();
            //_characterStore.BuyCharacter += OnBuyCharacter;
            _movePointController = _sceneInitializer.GetMovePointController();

            _pointUpgradePanel.Initialize();
            _pointUpgradePanel.OnSelectedButton+=(BuyPointUp);
            
            //_movePointController.OnClickWorkpoint += OnClickWorkpoint;
            //_movePointController.OnSelectedNewPoint+=OnSelectedNewPoint;
            //_movePointController.OnUnSelectedPoint+=OnUnSelectedPoint;
            _characterStoreRotation.gameObject.SetActive(!_characterStoreRotation.gameObject.activeSelf);
        }

        private void OnSelectMedicineBox()
        {
            if (!_selectedWorkPoint.IsHaveMedicineBox&&!_selectedWorkPoint.IsHaveWeaponBox)
            {
                if (_saveLoadService.MoneyData.IsMoneyEnough(_medicineBoxPrice))
                {
                    _saveLoadService.MoneyData.SpendMoney(_medicineBoxPrice);
                    _selectedWorkPoint.SetMedicineBox(_boxFactory.CreateMedicine());
                    _additionalEquipmentButton.SwitchStateButton(false);
                }

            }
        }

        private void OnSelectWeaponBox()
        {
            if (!_selectedWorkPoint.IsHaveWeaponBox&&!_selectedWorkPoint.IsHaveMedicineBox)
            {
                if (_saveLoadService.MoneyData.IsMoneyEnough(_weaponBoxPrice))
                {
                    _saveLoadService.MoneyData.SpendMoney(_weaponBoxPrice);
                    _selectedWorkPoint.SetWeaponBox(_boxFactory.CreateWeapon());
                    _additionalEquipmentButton.SwitchStateButton(false);
                }

            }
        }

        protected override void OnDisabled()
        {
            _saveLoadService.OnSelectedNewPoint -= CheckPointInfo;
            _characterStore.OnCharacterBought -= OnCharacterBought;
            _characterStore.OnMoneyEmpty -= ShowPanelAdsForMoney;
            _eliteCharacterStore.OnCharacterBought -= OnCharacterBought;
        }
        
        
        private void OnCharacterBought(Humanoid humanoid)
        {
            SwitchStorePanel();
        }
        
        private void ShowPanelAdsForMoney()
        {
            _applyAdsMoneyWindow.gameObject.SetActive(!_applyAdsMoneyWindow.gameObject);
            _characterStore.gameObject.SetActive(!_characterStore.gameObject.activeSelf);
            _characterStoreRotation.gameObject.SetActive(!_characterStoreRotation.gameObject.activeSelf);
            _eliteCharacterStore.gameObject.SetActive(!_eliteCharacterStore.gameObject.activeSelf);
            _buttonPanel.gameObject.SetActive(!_buttonPanel.gameObject.activeSelf);
        }

        private void InitializeButton()
        {
            _buttonStorePanel.onClick.AddListener(SwitchStorePanel);
            _closeButton.onClick.AddListener(SwitchStorePanel);
            _buttonRightPanel.onClick.AddListener(ChangeStateButtonPanel);
            _applyAdsMoneyWindowButton.onClick.AddListener(ShowPanelAds);
            _closeAdsMoneyWindowButton.onClick.AddListener(ShowPanelAdsForMoney);
        }
        
        private void ShowPanelAds()
        {
            ShowPanelAdsForMoney();
            _storePanel.gameObject.SetActive(false);
            _adsStore.gameObject.SetActive(true);
        }

        private void CheckPointInfo(WorkPoint workPoint)
        {
                _selectedWorkPoint = workPoint;

                if (_selectedWorkPoint.Level <=maxLevel )
                {
                    _pointUpgradePanel.SwitchStateButton(true);
                }
                else
                {
                    _pointUpgradePanel.SwitchStateButton(false);
                }
                
        }

        private void BuyPointUp()
        {
            int price = _priceForWorkPointUp;

            if (_saveLoadService.MoneyData.IsMoneyEnough(price))
            {
                _saveLoadService.MoneyData.SpendMoney(price);
                _workPointGroup.UpLevel(_selectedWorkPoint);
                
                OnBoughtUpgrade?.Invoke(_selectedWorkPoint);
            }
            else
            {
                print("должен мигать кошелек");
            }
        }
        
        private void ClosePanel()
        {
           
        }

        public List<Humanoid> GetAvaibleCharacters()
        {
            return _characters;
        }

        public CharacterStore GetBuyedPanel()
        {
            return _characterStore;
        }

        public void SetButtonState(bool isActive)
        {
            _buttonStorePanel.gameObject.SetActive(isActive);
        }
        
        private void ChangeStateButtonPanel()
        {
            isButtonPanelOpen = !isButtonPanelOpen;
            _rightButtonPanel.gameObject.SetActive(isButtonPanelOpen);
        }

        private void Buy(int price)
        {
        }
        
        public void SwitchStorePanel()
        {
            _isPanelActive=!_isPanelActive;
            _timeManager.SetPaused(_isPanelActive);
            SwitchPanels(_isPanelActive);
            SwitchCameras(_isPanelActive);
            
        }

        private void SwitchCameras(bool isActive)
        {
            _cameraUI.gameObject.SetActive(isActive);
            _characterVisual.gameObject.SetActive(isActive);
            isActive = !isActive;
            _cameraPhysical.gameObject.SetActive(isActive);
        }
        
        private void SwitchPanels(bool isActive)
        {
            IsStoreActive(isActive);
            _storePanel.gameObject.SetActive(isActive);
            _characterStore.gameObject.SetActive(isActive);
            _characterStoreRotation.gameObject.SetActive(isActive);
            
            _dimImage.gameObject.SetActive(isActive);
            _controlPanel.gameObject.SetActive(!isActive);
        }

        public CharacterStore GetCharacterStore()
        {
            return _characterStore;
        }

        public CharacterStore GetVipCharacterStore()
        {
            return _eliteCharacterStore;
        }
    }
}