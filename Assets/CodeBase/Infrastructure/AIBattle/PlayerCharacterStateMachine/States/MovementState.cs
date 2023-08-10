using System.Collections;
using Humanoids.AbstractLevel;
using Infrastructure.Location;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine.States
{
    public class MovementState : State
    {
        private readonly WaitForSeconds _waitForSeconds = new(0.3f);
        private WorkPoint _point;
        private NavMeshAgent _agent;
        private PlayerCharacterAnimController _playerCharacterAnimController;
        private float _minDistance = 0.3f;
        private bool _reachedDestination = true;
        private bool _isSetDestination = false;
        private Humanoid _humanoid;
        private Coroutine _coroutine;

        private void Awake()
        {
            _humanoid=GetComponent<Humanoid>();
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
            _agent = GetComponent<NavMeshAgent>();
            _agent.stoppingDistance = 0f; // Задайте минимальную дистанцию остановки
        }

        public void SetNewPoint(WorkPoint newPoint)
        {
            if (_point!=null)
            {
                _point.RemoveHumanoid();
            }
            
            _point = newPoint;
            _reachedDestination = true;
            _isSetDestination = false;
            Move();
        }
        
        private void Move()
        {
            if (_point != null)
            {
                Vector3 targetPosition = _point.transform.position;
                _playerCharacterAnimController.OnShoot(false);
                _playerCharacterAnimController.OnMove(true);
                _agent.SetDestination(targetPosition);
                _humanoid.IsMoving(true);
                _isSetDestination = true;
                _coroutine=StartCoroutine(CheckDistance()) ;
            }
            else
            {
                print("Invalid point");
            }
        }
        
        private IEnumerator CheckDistance()
        {
            _reachedDestination = false;
            
            if (_point == null)
                yield return null;

            while (_reachedDestination==false)
            {
                float distance = Vector3.Distance(transform.position, _point.transform.position);
            
                if (distance <= _minDistance)
                {
                    _point.SetHumanoid(_humanoid);
                    PlayerCharactersStateMachine.EnterBehavior<SearchTargetState>();
                }
                
                yield return _waitForSeconds;
            }
        }
        protected override void OnDisable()
        {
            _reachedDestination = true;
            _isSetDestination = false;
            _humanoid.IsMoving(false);
            _playerCharacterAnimController.OnMove(false);
            StopCoroutine(CheckDistance());
            
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            
        }

        public override void ExitBehavior()
        {
            enabled = false;
        }
    }
}