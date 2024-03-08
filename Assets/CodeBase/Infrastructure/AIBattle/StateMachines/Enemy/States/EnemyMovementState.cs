using System;
using System.Collections;
using System.Collections.Generic;
using Animation;
using Characters.Humanoids.AbstractLevel;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Infrastructure.AIBattle.EnemyAI.States
{
    public class EnemyMovementState : EnemyState
    {

        private Humanoid _humanoid;
        private NavMeshAgent _agent;
        private float _stoppingDistance;
        private float _distance;
        private Animator _animator;
        private EnemyAnimController _enemyAnimController;
        private Enemy _enemy;
        private bool _isStopping;
        private Dictionary<int, float> _animInfo=new();
        private bool _isTargetSet = false;
        private float _trackingProbability = 0.5f;
        private bool _isWalking;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _enemyAnimController = GetComponent<EnemyAnimController>();
            _isWalking = false;
            _enemy = GetComponent<Enemy>();
            _enemy.OnDeath += OnDeath;
        }

        private void OnDeath(Enemy enemy)
        {
            StopCoroutine(CheckDistance());
        }

        public override void OnTakeGranadeDamage()
        {
            _agent.speed = 0;
            _isStopping = true;
            StateMachine.EnterBehavior<EnemyStunningState>();
        }
        
        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = 1;
            _stoppingDistance = _enemy.GetRangeAttack();
            _agent.stoppingDistance=_stoppingDistance;
            _isStopping = false;
           // StopRandomly();
           saveLoadService.OnSetActiveHumanoid+=OnSetActiveHumanoid;
        }
        
        public void InitHumanoid(Humanoid targetHumanoid)
        {
            _humanoid = targetHumanoid;
            _humanoid.OnMove += OnTargetChangePoint;
        }

        private void OnSetActiveHumanoid()
        {
            ChangeState<EnemySearchTargetState>();
        }

        protected override void FixedUpdateCustom()
        {
            if (_isTargetSet==false)
            {
                Move();
            }
        }

        private void Move()
        {
    
            if (_humanoid != null && _humanoid.IsLife)
            {
                
                if (_agent.isOnNavMesh)
                    {
                        _agent.SetDestination(_humanoid.transform.position);
                        _isTargetSet = true;
                        _agent.isStopped = false;
                        _animator.SetBool(_enemyAnimController.Walk, true);
                        StartCoroutine(CheckDistance());
                    }
            }
            else
            {
                ChangeState<EnemySearchTargetState>();
            }
        }

        private void ChangeState<TState>() where TState : IEnemySwitcherState
        {
            StopCoroutine(CheckDistance());
            StopCoroutine(CheckSoldierPosition());
            
            if (_agent==null )
            {
                _agent = GetComponent<NavMeshAgent>();
            }
            if (_agent.isOnNavMesh)
            {
                _agent.isStopped = true;
            }
            _animator.SetBool(_enemyAnimController.Walk, false);
            _isTargetSet = false;
            _isWalking=false;
            StateMachine.EnterBehavior<TState>();
        }
        
        private IEnumerator CheckDistance()
        {
            while (_humanoid.IsLife&&_enemy.IsLife())
            {
                if (!_humanoid.IsLife)
                {
                    _animator.SetBool(_enemyAnimController.Walk, false);
                    ChangeState<EnemySearchTargetState>();
                    yield break;
                }
                else
                {
                    _distance = Vector3.Distance(transform.position, _humanoid.transform.position);
                    _agent.SetDestination(_humanoid.transform.position);
                    if (_stoppingDistance >= _distance)
                    {
                        _animator.SetBool(_enemyAnimController.Walk, false);
                        ChangeState<EnemyAttackState>();
                        yield break;
                    }
                    //
                    // {
                    //     CheckSoldierPosition();
                    // }
                }
                

                

                if (!_isWalking)
                {
                    _animator.SetBool(_enemyAnimController.Walk, true);
                    _isWalking=true;
                }
                 
                 
                yield return new WaitForSeconds(0.5f);
            }

            ChangeState<EnemySearchTargetState>();
        }
        
       private void OnTargetChangePoint()
       {
           if (gameObject.activeInHierarchy&_agent.isOnNavMesh)// Проверка активности объекта
           {
               if (ShouldTrackSoldier())
                   StartCoroutine(CheckSoldierPosition());
               else
                   ChangeState<EnemyAttackState>();
           }
       }
        
       private IEnumerator CheckSoldierPosition()
       {
           while ( _humanoid.IsMove)
           {
               Vector3 soldierPosition = _humanoid.transform.position;
               if (_agent.isOnNavMesh)
               {
                   _agent.SetDestination(soldierPosition);
               }
               
               
               yield return new WaitForSeconds(0.5f);
           }
           //
           // if (_agent.isOnNavMesh)
           // {
           //     _agent.isStopped = false;
           // }
           
           _animator.SetBool(_enemyAnimController.Walk, true);
           _isWalking=true;
       }

       private bool ShouldTrackSoldier() {     return Random.value <= _trackingProbability; }

        public void SetTarget(bool isTargetSet)
       {
           _isTargetSet = isTargetSet;
       }

        public override void Disable()
        {
            saveLoadService.OnSetActiveHumanoid-=OnSetActiveHumanoid;
        }
    }
}

