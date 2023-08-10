using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Service.SaveLoadService;
using UI.HUD.StorePanel;
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
        private WorkPoint _previousMovePoint;
        private WorkPoint _selectedPoint;
        public WorkPoint SelectedPoint => _selectedPoint;
        public WorkPoint MovePoint;
        private Humanoid _selectedHumanoid;
        public UnityAction<WorkPoint> OnClickWorkpoint;
        public UnityAction<WorkPoint> OnUnSelectedPoint;
        private bool isHumanoidSelected = false;
        private bool isPointToMoveTaked;
        private List<Button> _workPointButtons = new();
        public List<Button> GetWorkPointButtons => new List<Button>(_workPointButtons);
        [SerializeField] private WorkPoint _startPoint;
        private Store store;
        private SaveLoadService _saveLoadService;
        private bool isMovementOver = false;

        public void Initialize(SceneInitializer sceneInitializer, SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _sceneInitializer = sceneInitializer;
            _characterInitializer = sceneInitializer.GetPlayerCharacterInitializer();
            _activeHumanoids = _saveLoadService.GetActiveHumanoids();
            _workPointGroup = _characterInitializer.GetWorkPointGroup();
            FillWorkPoints();
            _workPointGroup.OnSelectedPoint += OnSelectedPoint;
            store = _sceneInitializer.GetStoreOnPlay();
            OnSelectedStartPoint();
            _workPoints[0].SetStartPointer();
            
        }

        private void FillWorkPoints()
        {
            foreach (WorkPoint workPoint in _workPointGroup.GetWorkPoints())
            {
                _workPoints.Add(workPoint);
            }
        }

        private void OnSelectedStartPoint()
        {
            _selectedPoint = _workPoints[0];
            _previousMovePoint =  _workPoints[0]; //  _selectedPoint.SetSelected(true);
            _saveLoadService.SetSelectedPoint(_selectedPoint);
        }

        private void OnSelectedPoint(WorkPoint newPoint)
        {
            isHumanoidSelected = _saveLoadService.GetSelectedHumanoid();
            _selectedHumanoid = _saveLoadService.GetSelectedHumanoid();
            
            
            
                if (_selectedPoint != newPoint)
                {
                    if (isHumanoidSelected)
                    {
                        if (_selectedHumanoid.IsLife() && !_selectedHumanoid.IsMove)
                        {
                            isPointToMoveTaked = false;
                        }
                    }
                    else
                    {
                        _previousMovePoint=newPoint;
                    }
                    
                    _selectedPoint.SetSelected(false);
                    _selectedPoint = newPoint;
                    _saveLoadService.SetSelectedPoint(_selectedPoint);
                
                
                    if (newPoint.IsBusy == false && isHumanoidSelected && isPointToMoveTaked == false)
                    {
                        isPointToMoveTaked = true;
                        MovePoint = newPoint;
                    }
                }
                else if (isHumanoidSelected)
                {
                    if (_selectedHumanoid.IsLife()&&!_selectedHumanoid.IsMove)
                    {
                        if (newPoint.IsBusy == false && isPointToMoveTaked == true)
                        {
                            _previousMovePoint.SetBusy(false);
                            newPoint.SetBusy(true);
                            newPoint.SelectedForMove(true);
                            _previousMovePoint = MovePoint;
               
                            PlayerCharactersStateMachine stateMachine =
                                _selectedHumanoid.GetComponent<PlayerCharactersStateMachine>();
                            stateMachine.EnterBehavior<MovementState>();
                            MovementState movementState = _selectedHumanoid.GetComponent<MovementState>();
                            movementState.SetNewPoint(newPoint);
                    
                        
                        
                            isPointToMoveTaked = false;
                        }
                    }
                }
                

                store.SetButtonState(_selectedPoint.IsBusy == false);
        }
    }
}