using System;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Data;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.Inits;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Infrastructure.Points;
using UI.Buttons;

namespace UI.HUD.StorePanel
{
    [RequireComponent(typeof(Wallet))]
    [RequireComponent(typeof(BoxStore))]
    public class Store : MonoCache
    {
        [SerializeField] private GameObject _storePanel;
        [SerializeField] private CharacterStore _characterStore;
        [SerializeField] private CharacterStore _eliteCharacterStore;
        [SerializeField] private GameObject _buttonGroup;
        [SerializeField] private AdsStore _adsStore;
        [SerializeField] private GameObject _applyAdsMoneyWindow;
        [SerializeField] CharacterStoreRotation _characterStoreRotation;
        [SerializeField] private Button _buttonStorePanel;
        [SerializeField] private Button _buttonRightPanel;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _applyAdsMoneyWindowButton;
        [SerializeField] private Button _closeAdsMoneyWindowButton;
        [SerializeField] private ButtonPanel _buttonPanel;
        [SerializeField] private GameObject _rightButtonPanel;
        [SerializeField] private WorkPointUpgradePanel _pointUpgradePanel;
        [SerializeField] private int _priceForWorkPointUp;
        [SerializeField] private Image _dimImage;
        [SerializeField] private Camera _cameraPhysical;
        [SerializeField] private Camera _cameraUI;
        [SerializeField] private Camera _characterVisual;
        private Wallet _wallet;

        private int _moneyAmount;
        private WorkPointGroup _workPointGroup;
        private bool isButtonPanelOpen = true;
        private List<Character> _characters = new();
        private SceneInitializer _sceneInitializer;
        private PlayerCharacterInitializer _characterInitializer;
        private MovePointController _movePointController;
        private SaveLoadService _saveLoadService;
        private int maxLevel = 3;
        private bool _isPanelActive = false;
        public Action<bool> IsStoreActive;
        public Action<WorkPoint> OnBoughtUpgrade;
        private GlobalTimer _globalTimer;
        private int _medicineBoxPrice = 100;
        private int _weaponBoxPrice = 200;
        private WorkPoint _selectedWorkPoint;
        private BoxStore _boxStore;
        public event Action <BoxData> OnBoughtBox;
        public event Action <CharacterData> OnBoughtCharacter;
        public void Initialize(SceneInitializer initializer, SaveLoadService saveLoadService, GlobalTimer globalTimer)
        {
            _wallet = GetComponent<Wallet>();
            _boxStore = GetComponent<BoxStore>();
            
            
            _globalTimer = globalTimer;
            _storePanel.gameObject.SetActive(!_storePanel.activeSelf);
            _saveLoadService = saveLoadService;
            _sceneInitializer = initializer;
            _moneyAmount = _saveLoadService.ReadAmountMoney();
            _workPointGroup = _sceneInitializer.GetPlayerCharacterInitializer().GetWorkPointGroup();
            _boxStore.Initialize(_saveLoadService,_wallet);
            SetCharacterInitializer();
            _storePanel.gameObject.SetActive(!_storePanel.activeSelf);
            _adsStore.Initialize(_wallet);
            _wallet.Initialize(_saveLoadService);
            
            AddListener();
        }

        private void SetCharacterInitializer()
        {
            _characterInitializer = _sceneInitializer.GetPlayerCharacterInitializer();
            //_characterInitializer.OnClickWorkpoint += CheckPointInfo;
            _characters = _saveLoadService.GetAvailableCharacters();
            
            _characterStore.Initialize( this);
            
            //  _eliteCharacterStore.Initialize(_saveLoadService,this);
            //_eliteCharacterStore.OnCharacterBought += OnCharacterBought;
            _movePointController = _sceneInitializer.GetMovePointController();

            _pointUpgradePanel.Initialize();

            //_movePointController.OnClickWorkpoint += OnClickWorkpoint;
            //_movePointController.OnSelectedNewPoint+=OnSelectedNewPoint;
            //_movePointController.OnUnSelectedPoint+=OnUnSelectedPoint;
            _characterStoreRotation.gameObject.SetActive(!_characterStoreRotation.gameObject.activeSelf);
        }

        protected override void OnDisabled() => RemoveListener();

        private void BuyCharacter(CharacterData data)
        {
            OnBoughtCharacter?.Invoke(data);
            SwitchStorePanel();
        }

        private void ShowPanelAdsForMoney()
        {
            _applyAdsMoneyWindow.gameObject.SetActive(!_applyAdsMoneyWindow.gameObject);
            _characterStore.gameObject.SetActive(!_characterStore.gameObject.activeSelf);
            _characterStoreRotation.gameObject.SetActive(!_characterStoreRotation.gameObject.activeSelf);
            _eliteCharacterStore.gameObject.SetActive(!_eliteCharacterStore.gameObject.activeSelf);
            _buttonGroup.gameObject.SetActive(!_buttonGroup.gameObject.activeSelf);
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

            if (_selectedWorkPoint.Level <= maxLevel)
            {
                _pointUpgradePanel.SwitchStateButton(true);
            }
            else
            {
                _pointUpgradePanel.SwitchStateButton(false);
            }

            ChangeButtonStoreState(!_selectedWorkPoint.IsBusy);
           
        }

        private void BuyPointUp()
        {
            int price = _priceForWorkPointUp;

            if (_wallet.IsMoneyEnough(price))
            {
                _wallet.SpendMoney(price);
                _workPointGroup.UpLevel(_selectedWorkPoint);

                //OnBoughtUpgrade?.Invoke(_selectedWorkPoint);
            }
            else
            {
                print("должен мигать кошелек");
            }
        }

        public List<Character> GetAvaibleCharacters() => _characters;

        public CharacterStore GetBuyedPanel() => _characterStore;

        public void ChangeButtonStoreState(bool isActive) => _buttonStorePanel.gameObject.SetActive(isActive);

        private void ChangeStateButtonPanel()
        {
            isButtonPanelOpen = !isButtonPanelOpen;
            _rightButtonPanel.gameObject.SetActive(isButtonPanelOpen);
        }

        public void SwitchStorePanel()
        {
            _isPanelActive = !_isPanelActive;
            _globalTimer.SetPaused(_isPanelActive);
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
            _storePanel.gameObject.SetActive(isActive);
            _characterStore.gameObject.SetActive(isActive);
            _characterStoreRotation.gameObject.SetActive(isActive);
            IsStoreActive.Invoke(isActive);

            _dimImage.gameObject.SetActive(isActive);
            _buttonPanel.SwitchPanelState();
        }

        public CharacterStore GetCharacterStore() => _characterStore;
        public BoxStore GetBoxStore() => _boxStore;

        public CharacterStore GetVipCharacterStore() => _eliteCharacterStore;

        public Wallet GetWallet() => _wallet;
        
        private void AddListener()
        {
            _saveLoadService.OnSelectedNewPoint += CheckPointInfo;
            _characterStore.OnMoneyEmpty += ShowPanelAdsForMoney;
            
            _pointUpgradePanel.OnSelectedButton += (BuyPointUp);
            
            _buttonStorePanel.onClick.AddListener(SwitchStorePanel);
            _closeButton.onClick.AddListener(SwitchStorePanel);
            _buttonRightPanel.onClick.AddListener(ChangeStateButtonPanel);
            _applyAdsMoneyWindowButton.onClick.AddListener(ShowPanelAds);
            _closeAdsMoneyWindowButton.onClick.AddListener(ShowPanelAdsForMoney);

            _boxStore.BuyBox+=OnBoughtBox;
            _characterStore.BuyCharacter += BuyCharacter;
        }

        private void RemoveListener()
        {
            _saveLoadService.OnSelectedNewPoint -= CheckPointInfo;
            _characterStore.BuyCharacter -= BuyCharacter;
            _characterStore.OnMoneyEmpty -= ShowPanelAdsForMoney;
            _eliteCharacterStore.BuyCharacter -= BuyCharacter;
            _pointUpgradePanel.OnSelectedButton -= (BuyPointUp);
        }
    }
}