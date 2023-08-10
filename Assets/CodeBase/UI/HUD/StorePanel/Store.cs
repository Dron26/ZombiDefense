using System;
using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Service;
using Service.SaveLoadService;
using UI.SceneBattle.Store;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.HUD.StorePanel
{
    public class Store : MonoCache
    {
        [SerializeField] private CharacterStore _characterStore;
        
        [SerializeField] private Button _buttonStorePanel;
        [SerializeField] private Button _buttonRightPanel;
        [SerializeField] private Button _closeButton;
        
        [SerializeField] private GameObject _controlPanel;
        [SerializeField] private GameObject _buttonPanel;
        [SerializeField] private WorkPointUpgradePanel _pointUpgradePanel;
        [SerializeField] private WorkPointGroup _workPointGroup;
        [SerializeField] private int _priceForWorkPointUp;
       
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
        private Wallet _wallet;

        public UnityAction<bool> IsStoreActive;
        public Action<WorkPoint> OnBoughtUpgrade;

        public void Initialize(SceneInitializer initializer, SaveLoadService saveLoadService, Wallet wallet)
        {
            _saveLoadService = saveLoadService;
            _sceneInitializer = initializer;
            _wallet=wallet;
            SetCharacterInitializer();
        }

        private void SetCharacterInitializer()
        {
            _characterInitializer = _sceneInitializer.GetPlayerCharacterInitializer();
            //_characterInitializer.OnClickWorkpoint += CheckPointInfo;
            _characters = _saveLoadService.GetAvailableCharacters();
            _saveLoadService.OnSelectedNewPoint += CheckPointInfo;
            _characterStore.Initialize(_saveLoadService,this,_wallet);
            _characterStore.OnCharacterBought += OnCharacterBought;
            InitializeButton();
            //_characterStore.BuyCharacter += OnBuyCharacter;
            _movePointController = _sceneInitializer.GetMovePointController();
           
            _pointUpgradePanel.Initialize(_characterInitializer, _saveLoadService);
            _pointUpgradePanel.GetButton().onClick.AddListener(BuyPointUp);
            
            //_movePointController.OnClickWorkpoint += OnClickWorkpoint;
            //_movePointController.OnSelectedNewPoint+=OnSelectedNewPoint;
            //_movePointController.OnUnSelectedPoint+=OnUnSelectedPoint;
        }

        private void OnCharacterBought(Humanoid humanoid)
        {
            SwitchStorePanel();
        }

        private void InitializeButton()
        {
            _closeButton.onClick.AddListener(SwitchStorePanel);
            _buttonStorePanel.onClick.AddListener(SwitchStorePanel);
            _buttonRightPanel.onClick.AddListener(ChangeStateButtonPanel);
        }

        private void CheckPointInfo(WorkPoint workPoint)
        {
           // bool isStartPoint = false;

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

            if (_wallet.CheckPossibilityBuy(price))
            {
                _wallet.SpendMoney(price);
                _workPointGroup.UpLevel(_selectedWorkPoint);
                
                OnBoughtUpgrade?.Invoke(_selectedWorkPoint);
            }
            else
            {
                print("должен мигать кошелек");
            }
        }

        private void ShowPanel()
        {
            _characterStore.gameObject.SetActive(true);
            // _characterStore.ShowAvaibleCharacters();
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
            _buttonPanel.gameObject.SetActive(isButtonPanelOpen);
        }

        private void Buy(int price)
        {
        }
        
        public void SwitchStorePanel()
        {
            _isPanelActive=!_isPanelActive;
            SwitchPanels(_isPanelActive);
            SwitchCameras(_isPanelActive);

            float timeState = Time.timeScale;
            Time.timeScale = _isPanelActive ? 1 : timeState;
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
            _dimImage.gameObject.SetActive(isActive);
            isActive = !isActive;
            _controlPanel.gameObject.SetActive(isActive);
        }

        public CharacterStore GetCharacterStore()
        {
            return _characterStore;
        }
    }
}