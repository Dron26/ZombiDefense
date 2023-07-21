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
        [SerializeField] private Image _dimImage;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _buttonRightPanel;
        [SerializeField] private CharacterStorePanelInfo _storePanelInfo;
        [SerializeField] private GameObject _buttonPanel;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private WorkPointGroup _workPointGroup;
        [SerializeField] private GameObject _controlPanel;
        [SerializeField] private List<int> _priceForWorkPointUp;
        [SerializeField] private Camera _cameraPhysical;
        [SerializeField] private Camera _cameraUI;
        [SerializeField] private Camera _characterVisual;

        private bool isButtonPanelOpen = true;
        private WorkPoint _selectedWorkPoint;
        private List<Humanoid> _characters = new();
        private SceneInitializer _sceneInitializer;
        private PlayerCharacterInitializer _characterInitializer;
        private MovePointController _movePointController;
        public UnityAction<Humanoid> BuyCharacter;
        private SaveLoad _saveLoad;
        private int maxLevel = 3;
        private bool _isPanelActive=false;

        public UnityAction<bool> IsStoreActive;
        public void Initialize(SceneInitializer initializer, SaveLoad saveLoad)
        {
            _saveLoad = saveLoad;
            _sceneInitializer = initializer;
            SetCharacterInitializer();
        }

        private void SetCharacterInitializer()
        {
            _characterInitializer = _sceneInitializer.GetPlayerCharacterInitializer();
            //_characterInitializer.OnClickWorkpoint += CheckPointInfo;
            _characters = _saveLoad.GetAvailableCharacters();
            _saveLoad.OnSelectedNewPoint += CheckPointInfo;
            _characterStore.Initialize(_saveLoad,this);
            InitializeButton();
            //_characterStore.BuyCharacter += OnBuyCharacter;
            _movePointController = _sceneInitializer.GetMovePointController();
            _storePanelInfo.Initialize(_characterInitializer, _saveLoad);
            _storePanelInfo.GetButton().onClick.AddListener(BuyPointUp);
            //_movePointController.OnClickWorkpoint += OnClickWorkpoint;
            //_movePointController.OnSelectedNewPoint+=OnSelectedNewPoint;
            //_movePointController.OnUnSelectedPoint+=OnUnSelectedPoint;
        }

        private void OnBuyCharacter(Humanoid humanoid)
        {
            BuyCharacter?.Invoke(humanoid);
            ClosePanel();
        }

        private void InitializeButton()
        {
            _closeButton.onClick.AddListener(SwithStorePanel);
            _buttonStorePanel.onClick.AddListener(SwithStorePanel);
            _buttonRightPanel.onClick.AddListener(ChangeStateButtonPanel);
        }

        private void CheckPointInfo(WorkPoint workPoint)
        {
            bool isStartPoint = false;

            if (isStartPoint)
            {
                _selectedWorkPoint = workPoint;

                if (_selectedWorkPoint.Level < _workPointGroup.MaxCountPrecent)
                {
                    _storePanelInfo.ShowButton(true);
                }
                else
                {
                    _storePanelInfo.ShowButton(false);
                }
            }
            else
            {
                isStartPoint = true;
            }
        }

        private void BuyPointUp()
        {
            int price = _priceForWorkPointUp[_selectedWorkPoint.Level];

            if (_wallet.CheckPossibilityBuy(price))
            {
                _wallet.SpendMoney(price);
                _workPointGroup.UpLevel(_selectedWorkPoint);
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
        
        public void SwithStorePanel()
        {
            _isPanelActive=!_isPanelActive;
            SwithPanels(_isPanelActive);
            SwithCameras(_isPanelActive);
        }

        private void SwithCameras(bool isActive)
        {
            _cameraUI.gameObject.SetActive(isActive);
            _characterVisual.gameObject.SetActive(isActive);
            isActive = !isActive;
            _cameraPhysical.gameObject.SetActive(isActive);
        }
        
        private void SwithPanels(bool isActive)
        {
            IsStoreActive(isActive);
            _dimImage.gameObject.SetActive(isActive);
            _storePanelInfo.gameObject.SetActive(isActive);
            isActive = !isActive;
            _controlPanel.gameObject.SetActive(isActive);
        }
    }
}