using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Service.SaveLoadService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.SceneBattle.Store
{
    public class StoreOnPlay : MonoCache
    {
        [SerializeField] private Panel _storePanel;
        [SerializeField] private Button _buttonSelectionPanel;
        [SerializeField] private Image _dimImage;
        [SerializeField] private Button _buttonRightPanel;
        [SerializeField] private CharacterStorePanelInfo _storePanelInfo;
        [SerializeField] private GameObject _rightPanel;
        
        private bool isRightPanelOpen = true;
        private Button _closeButton;
        private WorkPoint _selectedWorkPoint;
        private List<Humanoid> _characters = new();
        private SceneInitializer _sceneInitializer;
        private PlayerCharacterInitializer _characterInitializer;
        private MovePointController _movePointController; 
        public UnityAction<Humanoid> BuyCharacter;
        private SaveLoad _saveLoad;

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
            _storePanel.Initialize(_characterInitializer, this);
            InitializeButton();
            _storePanel.gameObject.SetActive(false);
            _storePanel.BuyCharacter += OnBuyCharacter;
            _movePointController=_sceneInitializer.GetMovePointController();
            _storePanelInfo.Initialize(_characterInitializer, _saveLoad);
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
            _selectedWorkPoint=workPoint;
            _buttonSelectionPanel.gameObject.SetActive(true);
        }

        private void ShowPanel()
        {
            _storePanel.gameObject.SetActive(true);
            _storePanel.ShowAvaibleCharacters();
        }

        private void ClosePanel()
        {
            _dimImage.gameObject.SetActive(false);
            _storePanel.gameObject.SetActive(false);
        }

        public List<Humanoid> GetAvaibleCharacters()
        {
            return _characters;
        }

        public Panel GetBuyedPanel()
        {
            return _storePanel;
        }

        public void SetButtonState(bool isActive)
        {
            _buttonSelectionPanel.gameObject.SetActive(isActive);
        }

        public void SetPanelInfoState()
        {
            if (_storePanel.gameObject.activeSelf)
            {
                _storePanel.gameObject.SetActive( false);
                _dimImage.gameObject.SetActive( false);
            }
            else
            {
                _storePanel.gameObject.SetActive( true);
                _dimImage.gameObject.SetActive( true);
            }
        }

        private void ChangeStateRightPanel()
        {
            isRightPanelOpen=!isRightPanelOpen;
            _rightPanel.gameObject.SetActive(isRightPanelOpen);
        }
    }
}