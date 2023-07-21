using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Service;
using Service.SaveLoadService;
using UI.HUD.StorePanel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Upgrades;

namespace UI.SceneBattle.Store
{
    public class StoreOnPlay : MonoCache
    {
        [SerializeField] private CharacterStorePanel _storeCharacterStorePanel;
        [SerializeField] private Button _buttonSelectionPanel;
        [SerializeField] private Image _dimImage;
        [SerializeField] private Button _buttonRightPanel;
        [SerializeField] private CharacterStorePanelInfo _storePanelInfo;
        [SerializeField] private GameObject _rightPanel;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private List<int> _priceForWorkPointUp;
        [SerializeField] private WorkPointGroup _workPointGroup;
        [SerializeField] private UpgradeManager _upgradeManager;
        private bool isRightPanelOpen = true;
        private Button _closeButton;
        private WorkPoint _selectedWorkPoint;
        private List<Humanoid> _characters = new();
        private SceneInitializer _sceneInitializer;
        private PlayerCharacterInitializer _characterInitializer;
        private MovePointController _movePointController; 
        public UnityAction<Humanoid> BuyCharacter;
        private SaveLoad _saveLoad;
        private int maxLevel = 3;

        
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
            _characters=_saveLoad.GetAvailableCharacters();
            _saveLoad.OnSelectedNewPoint += CheckPointInfo;
                // _storeCharacterStorePanel.Initialize(_characterInitializer, this);
            InitializeButton();
            _storeCharacterStorePanel.gameObject.SetActive(false);
            _storeCharacterStorePanel.BuyCharacter += OnBuyCharacter;
            _movePointController=_sceneInitializer.GetMovePointController();
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
            _closeButton = _dimImage.GetComponent<Button>();
            _closeButton.onClick.AddListener(ClosePanel);
            _buttonSelectionPanel.onClick.AddListener(SetPanelInfoState);
            _buttonRightPanel.onClick.AddListener(ChangeStateRightPanel);
        }

        private void CheckPointInfo(WorkPoint workPoint)
        {
            bool isStartPoint = false;

            if (isStartPoint)
            {
                _selectedWorkPoint=workPoint;

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
            _storeCharacterStorePanel.gameObject.SetActive(true);
            _storeCharacterStorePanel.ShowAvaibleCharacters();
        }

        private void ClosePanel()
        {
            _dimImage.gameObject.SetActive(false);
            _storeCharacterStorePanel.gameObject.SetActive(false);
        }

        public List<Humanoid> GetAvaibleCharacters()
        {
            return _characters;
        }

        public CharacterStorePanel GetBuyedPanel()
        {
            return _storeCharacterStorePanel;
        }

        public void SetButtonState(bool isActive)
        {
            _buttonSelectionPanel.gameObject.SetActive(isActive);
        }

        public void SetPanelInfoState()
        {
            if (_storeCharacterStorePanel.gameObject.activeSelf)
            {
                _storeCharacterStorePanel.gameObject.SetActive( false);
                _dimImage.gameObject.SetActive( false);
                _storePanelInfo.gameObject.SetActive( false);
            }
            else
            {
                _storeCharacterStorePanel.gameObject.SetActive( true);
                _dimImage.gameObject.SetActive( true);
                _storePanelInfo.gameObject.SetActive( true);
                _upgradeManager.Initialize(_saveLoad);
            }
        }

        private void ChangeStateRightPanel()
        {
            isRightPanelOpen=!isRightPanelOpen;
            _rightPanel.gameObject.SetActive(isRightPanelOpen);
        }

        private void Buy(int price)
        {

            
        }
    }
}