using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Animation;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.EnemyAI.States;
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
        private Vector3 _humanoidPosition ;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _enemyAnimController = GetComponent<EnemyAnimController>();
            
            _enemy = GetComponent<Enemy>();
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
            _humanoidPosition = _humanoid.transform.position;
            _humanoid.OnMove += OnTargetChangePoint;
        }

        private void OnSetActiveHumanoid()
        {
            ChangeState();
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
                        _agent.SetDestination(_humanoidPosition);
                        _isTargetSet = true;
                        Movement();
                    }
            }
            else
            {
                ChangeState();
            }
        }
        
        private void Movement()
        {
            _agent.isStopped = false;
            _animator.SetBool(_enemyAnimController.Walk, true);
           
            StartCoroutine(CheckDistance());;
        }

        private void ChangeState()
        {
            if (_agent==null )
            {
                _agent = GetComponent<NavMeshAgent>();
            }
            if (_agent.isOnNavMesh)
            {
                _agent.isStopped = true;
            }
            _animator.SetBool(_enemyAnimController.Walk, false);
            SetTarget(false);
            StateMachine.EnterBehavior<EnemySearchTargetState>();
        }
        
        private IEnumerator CheckDistance()
        {
            while (_isTargetSet &&_humanoid.IsLife)
            {
                _distance = Vector3.Distance(transform.position, _humanoid.transform.position);

                if (_stoppingDistance >= _distance)
                {
                    _animator.SetBool(_enemyAnimController.Walk, false);
                    StateMachine.EnterBehavior<EnemyAttackState>();
                }

                if (!_humanoid.IsLife)
                {
                    _animator.SetBool(_enemyAnimController.Walk, false);
                    SetTarget(false);
                    StateMachine.EnterBehavior<EnemySearchTargetState>();
                }
               
           
                yield return new WaitForSeconds(0.5f);
            }

            ChangeState();
        }
        
       private void OnTargetChangePoint()
       {
           if (gameObject.activeInHierarchy&_agent.isOnNavMesh)// Проверка активности объекта
           {
               if (ShouldTrackSoldier())
                   StartCoroutine(CheckSoldierPosition());
               else
                   ChangeState();
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
               
               
               yield return new WaitForSeconds(1.5f);
           }
           
           _agent.isStopped = false;
           _animator.SetBool(_enemyAnimController.Walk, true);
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

