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
using UI.Resurse;
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
        
        [SerializeField] private Button _specialCar;
        [SerializeField] private Button _specialCarButton;
        [SerializeField] private GameObject _specialTechniquePanel;
        
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
        private IUpgradeTree _upgradeTree;
        private ICharacterHandler _characterHandler;
        private int _priceCharacterLevelUp=1500;
        private int _priceSpecialTechnique;
        private int _precentDecreaseCostSpecialTechnique;
        private int _maxLevel;
        private int _precentLevelUp;

        public void Initialize(SceneInitializer initializer)
        {
            _wallet = GetComponent<Wallet>();
            _boxStore = GetComponent<BoxStore>();
            _pauseService = AllServices.Container.Single<IPauseService>();
            _eventBroadcaster=AllServices.Container.Single<IGameEventBroadcaster>(); 
            _moneyAmount = AllServices.Container.Single<ICurrencyHandler>().GetCurrentMoney();
            _characterHandler=AllServices.Container.Single<ICharacterHandler>();
            
            _storePanel.gameObject.SetActive(!_storePanel.activeSelf);
            _sceneInitializer = initializer;
            _workPointGroup = _sceneInitializer.GetPlayerCharacterInitializer().GetWorkPointGroup();
            _boxStore.Initialize(_wallet);
            SetCharacterInitializer();
            _storePanel.gameObject.SetActive(!_storePanel.activeSelf);
            // _adsStore.Initialize(_wallet);
            _wallet.Initialize();
            _upgradeTree=AllServices.Container.Single<IUpgradeTree>();
            SetUpgrades();
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

            if (_maxLevel> _selectedWorkPoint.Level && _wallet.IsMoneyEnough(price))
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
            _pauseService.ChangePause(_isPanelActive);
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
            _characterStore.OnReachLimitCharacter += IsReachLimitCharacter;
            _specialCar.onClick.AddListener(ShowSpecialTechniquePanel);
            _specialCarButton.onClick.AddListener(SetActiveSpecialTechnique);
            
          //  _healthRestoreButton.onClick.AddListener((HealthRestore) );
        }

     
        private void RemoveListener()
        {
            _eventBroadcaster.OnSelectedNewPoint -= CheckPointInfo;
            _characterStore.BuyCharacter -= BuyCharacter;
            _characterStore.OnMoneyEmpty -= ShowPanelAdsForMoney;
            _pointUpgradePanel.OnSelectedButton -= (BuyPointUp);
            _boxStore.BuyBox-=OnBuyBox;
            _specialCar.onClick.AddListener(ShowSpecialTechniquePanel);
            _specialCarButton.onClick.AddListener(SetActiveSpecialTechnique);
        }

        private void ShowSpecialTechniquePanel()
        {
            _specialTechniquePanel.gameObject.SetActive(true);
        }
        
        private void SetActiveSpecialTechnique()
        {
            int price = (int)Mathf.Round(_priceSpecialTechnique*(100-_precentDecreaseCostSpecialTechnique)/ 100);
            
            if ( _wallet.IsMoneyEnough(price))
            {
                _wallet.SpendMoney(price);

                _eventBroadcaster.InvokeOnActivatedSpecialTechnique();
            }
            else
            {
                print("должен мигать кошелек");
            }
            _eventBroadcaster.InvokeOnCharacterLevelUp();
        }

        private void IsReachLimitCharacter(bool isReach)
        {
            _buttonStorePanel.gameObject.SetActive(isReach);
        }

        public void OnBuyBox(BoxData data)
        {
            OnBoughtBox?.Invoke(data);
        }
        
        private void SetUpgrades()
        {
            UpdateUpgradeValue(UpgradeGroupType.Defence, UpgradeType.IncreaseMaxLevelDefensePoint, value => _maxLevel = value);
            UpdateUpgradeValue(UpgradeGroupType.Defence, UpgradeType.IncreaseMaxLevelDefensePoint, value => _precentLevelUp = value);
            UpdateUpgradeValue(UpgradeGroupType.SpecialTechnique, UpgradeType.AddSpecialTechnique, value => _priceSpecialTechnique = value);
            UpdateUpgradeValue(UpgradeGroupType.SpecialTechnique, UpgradeType.DecreaseCostSpecialTechnique, value => _precentDecreaseCostSpecialTechnique = value);

            _specialCar.gameObject.SetActive(_priceSpecialTechnique != 0);
        }

        private void UpdateUpgradeValue(UpgradeGroupType groupType, UpgradeType type, Action<int> setValue)
        {
            var upgrades = _upgradeTree.GetUpgradeValue(groupType, type);
            if (upgrades != null && upgrades.Count > 0)
            {
                setValue((int)Mathf.Round(upgrades[0]));
            }
        }
        
        private void CharacterLevelUp()
        {
            int price = _priceCharacterLevelUp;
            
            if ( _wallet.IsMoneyEnough(price))
            {
                _wallet.SpendMoney(price);
                _workPointGroup.UpLevel(_selectedWorkPoint);

                _eventBroadcaster.InvokeOnCharacterLevelUp();
            }
            else
            {
                print("должен мигать кошелек");
            }
            _eventBroadcaster.InvokeOnCharacterLevelUp();
        }
        
        private void HealthRestore()
        {
            int price = _characterInitializer.GetSelectedCharacter().Price/2;
            
            if ( _wallet.IsMoneyEnough(price))
            {
                _wallet.SpendMoney(price);
                _characterInitializer.GetSelectedCharacter().RestoreHealth();
            }
            else
            {
                _eventBroadcaster.InvokeOnMoneyEnough();
            }
        }
    }
}