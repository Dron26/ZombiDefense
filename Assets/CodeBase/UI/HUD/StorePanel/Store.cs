using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Humanoids.AbstractLevel;
using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.Inits;
using Infrastructure.Points;
using Interface;
using Services;
using Services.PauseService;
using Services.SaveLoad;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;
using CharacterData = Characters.Humanoids.AbstractLevel.CharacterData;

namespace UI.HUD.StorePanel
{
    [RequireComponent(typeof(Wallet))]
    [RequireComponent(typeof(BoxStore))]
    public class Store : MonoCache
    {
        [SerializeField] private GameObject _storePanel;
        [SerializeField] private CharacterStore _characterStore;
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
        private IPauseService _pauseService;
        private int _moneyAmount;
        private WorkPointGroup _workPointGroup;
        private bool isButtonPanelOpen = true;
        private List<Character> _characters = new();
        private SceneInitializer _sceneInitializer;
        private PlayerCharacterInitializer _characterInitializer;
        private MovePointController _movePointController;
        private int maxLevel = 3;
        private bool _isPanelActive = false;
        public Action<bool> IsStoreActive;
        public Action<WorkPoint> OnBoughtUpgrade;
        private int _medicineBoxPrice = 100;
        private int _weaponBoxPrice = 200;
        private WorkPoint _selectedWorkPoint;
        private BoxStore _boxStore;
        public event Action <BoxData> OnBoughtBox;
        public event Action <CharacterData> OnBoughtCharacter;
        private IGameEventBroadcaster _eventBroadcaster;

        public void Initialize(SceneInitializer initializer)
        {
            _wallet = GetComponent<Wallet>();
            _boxStore = GetComponent<BoxStore>();
            _pauseService = AllServices.Container.Single<IPauseService>();
            _eventBroadcaster=AllServices.Container.Single<IGameEventBroadcaster>(); 

            _storePanel.gameObject.SetActive(!_storePanel.activeSelf);
            _sceneInitializer = initializer;
            _moneyAmount = AllServices.Container.Single<ICurrencyHandler>().GetCurrentMoney();
            _workPointGroup = _sceneInitializer.GetPlayerCharacterInitializer().GetWorkPointGroup();
            _boxStore.Initialize(_wallet);
            SetCharacterInitializer();
            _storePanel.gameObject.SetActive(!_storePanel.activeSelf);
            // _adsStore.Initialize(_wallet);
            _wallet.Initialize();
            
            AddListener();
        }

        private void SetCharacterInitializer()
        {
            _characterInitializer = _sceneInitializer.GetPlayerCharacterInitializer();
            //_characterInitializer.OnClickWorkpoint += CheckPointInfo;
            _characters = AllServices.Container.Single<ICharacterHandler>().GetAvailableCharacter();
            
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
            _buttonGroup.gameObject.SetActive(!_buttonGroup.gameObject.activeSelf);
        }

        private void ShowPanelAds()
        {
            ShowPanelAdsForMoney();
            _storePanel.gameObject.SetActive(false);
            //  _adsStore.gameObject.SetActive(true);
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
            _pauseService.SetPause(_isPanelActive);
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
        
        public Wallet GetWallet() => _wallet;
        
        private void AddListener()
        {
            _eventBroadcaster.OnSelectedNewPoint += CheckPointInfo;
            _characterStore.OnMoneyEmpty += ShowPanelAdsForMoney;
            
            _pointUpgradePanel.OnSelectedButton += (BuyPointUp);
            
            _buttonStorePanel.onClick.AddListener(SwitchStorePanel);
            _closeButton.onClick.AddListener(SwitchStorePanel);
            _buttonRightPanel.onClick.AddListener(ChangeStateButtonPanel);
            _applyAdsMoneyWindowButton.onClick.AddListener(ShowPanelAds);
            _closeAdsMoneyWindowButton.onClick.AddListener(ShowPanelAdsForMoney);

            _boxStore.BuyBox+=OnBuyBox;
            _characterStore.BuyCharacter += BuyCharacter;
        }
        public void OnBuyBox(BoxData data)
        {
            OnBoughtBox?.Invoke(data);
        }
        private void RemoveListener()
        {
            _eventBroadcaster.OnSelectedNewPoint -= CheckPointInfo;
            _characterStore.BuyCharacter -= BuyCharacter;
            _characterStore.OnMoneyEmpty -= ShowPanelAdsForMoney;
            _pointUpgradePanel.OnSelectedButton -= (BuyPointUp);
            _boxStore.BuyBox+=OnBuyBox;
        }
    }
}