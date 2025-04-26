using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.AIBattle.StateMachines.Humanoid;
using Infrastructure.AIBattle.StateMachines.Humanoid.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.Inits;
using Interface;
using Services;
using Services.SaveLoad;
using UI.HUD.StorePanel;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Points
{
    public class MovePointController : MonoCache
    {
        public WorkPoint SelectedPoint => _selectedPoint;
        public WorkPoint PreviousMovePoint => _previousMovePoint;
        private PlayerCharacterInitializer _characterInitializer;
        private List<Character> _activeCharacters;
        private SceneInitializer _sceneInitializer;
        private WorkPointGroup _workPointGroup;
        private List<WorkPoint> _workPoints = new();
        private WorkPoint _previousMovePoint;
        private WorkPoint _selectedPoint;
        private WorkPoint _currentPoint;
        private WorkPoint _movePoint;
        private Character _selectedCharacter;
        private bool isChracterSelected = false;
        private bool isPointToMoveTaked;
        [SerializeField] private WorkPoint _startPoint;
        private Store store;
        private bool isMovementOver = false;
        private ICharacterHandler _characterHandler;
        private ILocationHandler _locationHandler;
        private IGameEventBroadcaster _eventBroadcaster;

        public void Initialize(SceneInitializer sceneInitializer )
        {
            _characterHandler=AllServices.Container.Single<ICharacterHandler>();
            _locationHandler=AllServices.Container.Single<ILocationHandler>();
            _eventBroadcaster=AllServices.Container.Single<IGameEventBroadcaster>(); 

            _sceneInitializer = sceneInitializer;
            _characterInitializer = sceneInitializer.GetPlayerCharacterInitializer();
            _activeCharacters = _characterHandler.GetActiveCharacters();
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
            _locationHandler.SetSelectedPointId(0);
            _eventBroadcaster.InvokeOnSelectedNewPoint(_selectedPoint);
        }

        public void OnSelectedPoint(WorkPoint newPoint)
        {
            _eventBroadcaster.InvokeOnSelectedNewPoint(newPoint);
            isChracterSelected = _characterHandler.GetSelectedCharacter();
            _selectedCharacter = _characterHandler.GetSelectedCharacter();


            if (_selectedPoint != newPoint)
            {
                //Debug.Log("selectNewPoint");
                _selectedPoint.SelectedForMove(false);

                if (newPoint.IsBusy)
                {
                    _previousMovePoint = newPoint;
                }
                else
                {
                    if (isChracterSelected)
                    {
                        if (_selectedCharacter.IsLife() && !_selectedCharacter.IsMove)
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
                _locationHandler.SetSelectedPointId(_selectedPoint.Id);


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
                if (_selectedCharacter.IsLife() && !_selectedCharacter.IsMove)
                {
//                    Debug.Log("selectOldPoint");
                    if (newPoint.IsBusy == false && isPointToMoveTaked)
                    {
//                        Debug.Log("movePoint");
                        _previousMovePoint.SetBusy(false);
                        newPoint.SetBusy(true);
                        newPoint.SelectedForMove(true);

                        PlayerCharactersStateMachine stateMachine =
                            _selectedCharacter.GetComponent<PlayerCharactersStateMachine>();
                        SetPoint(newPoint);
                        stateMachine.MoveTo();
                        stateMachine.EnterBehavior<MovementState>();
                        isPointToMoveTaked = false;
                    }
                }
            }


            store.ChangeButtonStoreState(_selectedPoint.IsBusy == false);
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