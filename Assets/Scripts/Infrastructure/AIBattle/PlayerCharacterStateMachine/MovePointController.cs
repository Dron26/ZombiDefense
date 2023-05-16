using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Service.SaveLoadService;
using UI.SceneBattle.Store;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine
{
    public class MovePointController : MonoCache
    {
        
        PlayerCharacterInitializer _characterInitializer;
        private List<Humanoid> _activeHumanoids;
        private SceneInitializer _sceneInitializer;
        private WorkPointGroup _workPointGroup;
        private List<WorkPoint> _workPoints = new();
        private WorkPoint _previousPoint;
        private WorkPoint _selectedPoint;
        public WorkPoint SelectedPoint=>_selectedPoint;
        public WorkPoint MovePoint;
        private Humanoid _selectedHumanoid;
        public UnityAction<WorkPoint> OnClickWorkpoint;
        public UnityAction<WorkPoint> OnSelectedNewPoint;
        public UnityAction<WorkPoint> OnUnSelectedPoint;
        private bool isHumanoidSelected = false;
        private bool isPointToMoveTaked;
        private List<Button> _workPointButtons = new();
        public List<Button> GetWorkPointButtons => new List<Button>(_workPointButtons);
        [SerializeField] private WorkPoint _startPoint;
        private StoreOnPlay _storeOnPlay;
        private SaveLoad _saveLoad;
        
        public void Initialize(SceneInitializer sceneInitializer, SaveLoad saveLoad)
        {
            _saveLoad=saveLoad;
            _sceneInitializer = sceneInitializer;
            _characterInitializer = sceneInitializer.GetPlayerCharacterInitializer();
            _activeHumanoids = _saveLoad.GetActiveHumanoids();
            _workPointGroup = _characterInitializer.GetWorkPointGroup();

            FillWorkPoints();
            _workPointGroup.OnSelectedPoint += OnSelectedPoint;
            _storeOnPlay = _sceneInitializer.GetStoreOnPlay();
            OnSelectedStartPoint(_startPoint);
        }

        private void FillWorkPoints()
        {
            foreach (WorkPoint workPoint in _workPointGroup.GetWorkPoints())
            {
                _workPoints.Add(workPoint);
            }
        }

        private void OnSelectedStartPoint(WorkPoint workPoint)
        {
            _selectedPoint = workPoint;
            _previousPoint = workPoint;
            _selectedPoint.SetSelected(true);
            _saveLoad.SetSelectedPOoint(_selectedPoint);
        }

        private void OnSelectedPoint(WorkPoint workPoint)
        {
            isHumanoidSelected = _saveLoad.GetSelectedHumanoid();

            if (_previousPoint != workPoint)
            {
                _previousPoint.SetSelected(false);
                _selectedPoint = workPoint;
                
                _saveLoad.SetSelectedPOoint(_selectedPoint);
                _selectedPoint.SetSelected(true);
                if (!workPoint.IsBusy && isHumanoidSelected&& isPointToMoveTaked==false)
                {
                    isPointToMoveTaked = true;
                    MovePoint = workPoint;
                    
                }
                    
                _storeOnPlay.SetButtonState(true);
            }
            else if (_previousPoint == workPoint && workPoint.IsBusy==false && isPointToMoveTaked==true)
            {

                _selectedHumanoid=_saveLoad.GetSelectedHumanoid();
               // Humanoid humanoid = _characterInitializer.GetSelectedCharacter();
                PlayerCharactersStateMachine stateMachine = _selectedHumanoid.GetComponent<PlayerCharactersStateMachine>();
                stateMachine.EnterBehavior<MovementState>();
            }
            else if (_previousPoint == workPoint && workPoint.IsBusy)
            {
                _storeOnPlay.SetButtonState(false);
                _storeOnPlay.SetPanelInfoState(true);
            }

            _previousPoint = _selectedPoint;
            //
            // if (_previousPoint != workPoint && workPoint.IsBusy)
            // {
            //     OnUnSelectedPoint?.Invoke(workPoint);
            // }

            if (_selectedHumanoid != null)
            {
                PlayerCharactersStateMachine stateMachine =
                    _selectedHumanoid.GetComponent<PlayerCharactersStateMachine>();
                stateMachine.SetMovePoint(MovePoint);
            }
        }
    }
}

