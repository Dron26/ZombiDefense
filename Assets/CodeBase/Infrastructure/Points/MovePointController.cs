using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.Inits;
using Service.SaveLoad;
using UI.HUD.StorePanel;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Points
{
    public class MovePointController : MonoCache
    {
        private PlayerCharacterInitializer _characterInitializer;
        private List<Character> _activeCharacters;
        private SceneInitializer _sceneInitializer;
        private WorkPointGroup _workPointGroup;
        private List<WorkPoint> _workPoints = new();
        private WorkPoint _previousMovePoint;
        private WorkPoint _selectedPoint;
        private WorkPoint _currentPoint;
        public WorkPoint SelectedPoint => _selectedPoint;
        private WorkPoint _movePoint;
        private Character _selectedCharacter;
        public UnityAction<WorkPoint> OnClickWorkpoint;
        public UnityAction<WorkPoint> OnUnSelectedPoint;
        private bool isChracterSelected = false;
        private bool isPointToMoveTaked;
        [SerializeField] private WorkPoint _startPoint;
        private Store store;
        private SaveLoadService _saveLoadService;
        private bool isMovementOver = false;

        public void Initialize(SceneInitializer sceneInitializer, SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _sceneInitializer = sceneInitializer;
            _characterInitializer = sceneInitializer.GetPlayerCharacterInitializer();
            _activeCharacters = _saveLoadService.GetActiveCharacters();
            _workPointGroup = _characterInitializer.GetWorkPointGroup();
            FillWorkPoints();
            _workPointGroup.OnSelectedPoint += OnSelectedPoint;
            store = _sceneInitializer.Window.GetStoreOnPlay();
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
            _previousMovePoint = _workPoints[0];
            _currentPoint = _workPoints[0]; //  _selectedPoint.SetSelected(true);
            _saveLoadService.SetSelectedPoint(_selectedPoint);
        }

        public void OnSelectedPoint(WorkPoint newPoint)
        {
            isChracterSelected = _saveLoadService.GetSelectedCharacter();
            _selectedCharacter = _saveLoadService.GetSelectedCharacter();


            if (_selectedPoint != newPoint)
            {
                Debug.Log("selectNewPoint");
                _selectedPoint.SelectedForMove(false);

                if (newPoint.IsBusy)
                {
                    _previousMovePoint = newPoint;
                }
                else
                {
                    if (isChracterSelected)
                    {
                        if (_selectedCharacter.IsLife && !_selectedCharacter.IsMove)
                        {
                            isPointToMoveTaked = false;
                        }
                        // if (newPoint.IsBusy)
                        // {
                        //     _previousMovePoint=newPoint;
                        // }
                    }
                    else
                    {
                        _previousMovePoint = newPoint;
                    }
                }


                _selectedPoint.SetSelected(false);
                _selectedPoint = newPoint;
                _saveLoadService.SetSelectedPoint(_selectedPoint);


                if (newPoint.IsBusy == false && isChracterSelected && isPointToMoveTaked == false)
                {
                    if (_selectedCharacter.CanMove)
                    {
                        isPointToMoveTaked = true;
                        _movePoint = newPoint;
                    }
                }
            }
            else if (isChracterSelected)
            {
                if (_selectedCharacter.IsLife && !_selectedCharacter.IsMove)
                {
                    Debug.Log("selectOldPoint");
                    if (newPoint.IsBusy == false && isPointToMoveTaked)
                    {
                        Debug.Log("movePoint");
                        _previousMovePoint.SetBusy(false);
                        newPoint.SetBusy(true);
                        newPoint.SelectedForMove(true);

                        PlayerCharactersStateMachine stateMachine =
                            _selectedCharacter.GetComponent<PlayerCharactersStateMachine>();
                        stateMachine.MoveTo();
                        stateMachine.EnterBehavior<MovementState>();
                        SetPoint(newPoint);
                        isPointToMoveTaked = false;
                    }
                    else
                    {
                    }
                }
            }


            store.SetButtonState(_selectedPoint.IsBusy == false);
        }

        public void SetPoint(WorkPoint newPoint)
        {
            MovementState movementState = _selectedCharacter.GetComponent<MovementState>();
            movementState.SetNewPoint(newPoint);
            ChangeCurrentPoint(newPoint);
        }

        private void ChangeCurrentPoint(WorkPoint newPoint)
        {
            _previousMovePoint.RemoveCharacter();
            _previousMovePoint = newPoint;
        }

        public void SetCurrentPoint(WorkPoint point)
        {
            _currentPoint = point;
            _previousMovePoint = point;
        }
    }
}