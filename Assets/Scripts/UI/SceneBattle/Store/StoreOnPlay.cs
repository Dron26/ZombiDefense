using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.SceneBattle.Store
{
    public class StoreOnPlay : MonoCache
    {
        [SerializeField] private Panel _storePanel;
        private Button _showButton;
        private Button _closeButton;
        private WorkPoint _selectedWorkPoint;
        [SerializeField] private Image _dimImage;
        private List<Humanoid> _characters = new();
        private List<WorkPoint> _points;
        private SceneInitializer _sceneInitializer;
        private PlayerCharacterInitializer _characterInitializer;
        [SerializeField] private Button _buttonSelectionPanel;
       private MovePointController _movePointController;
        
       public void Initialize(SceneInitializer initializer)
        {
            _sceneInitializer = initializer;
            SetCharacterInitializer();
        }

        private void SetCharacterInitializer()
        {
            _characterInitializer = _sceneInitializer.GetPlayerCharacterInitializer();
            //_characterInitializer.OnClickWorkpoint += CheckPointInfo;
            _characters = _sceneInitializer.GetAvaibelCharacters();
            _storePanel.Initialize(_characterInitializer, this);
            InitializeButton();
            _storePanel.gameObject.SetActive(false);
            
            _movePointController=_sceneInitializer.GetMovePointController();
            //_movePointController.OnClickWorkpoint += OnClickWorkpoint;
            //_movePointController.OnSelectedNewPoint+=OnSelectedNewPoint;
            //_movePointController.OnUnSelectedPoint+=OnUnSelectedPoint;
        }

        private void InitializeButton()
        {
            _closeButton = _dimImage.GetComponent<Button>();
            _closeButton.onClick.AddListener(ClosePanel);
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
            // _selectedWorkPoint.SetState(false);
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

        public void SetButton()
        {
            _buttonSelectionPanel.gameObject.SetActive(true);
        }
    }
}