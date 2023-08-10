using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Animation;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
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
        private bool _isHumanoidInstalled = false;
        private float _trackingProbability = 0.5f;
        private Vector3 _humanoidPosition ;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _enemyAnimController = GetComponent<EnemyAnimController>();
            _agent = GetComponent<NavMeshAgent>();
            _enemy = GetComponent<Enemy>();
            _agent.speed = 1;
        }

        private void Start()
        {
            _stoppingDistance = _enemy.GetRangeAttack();
            _agent.stoppingDistance=_stoppingDistance;
            _isStopping = false;
           // StopRandomly();
           saveLoadService.OnSetActiveHumanoid=OnSetActiveHumanoid;
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
            if (_isHumanoidInstalled==false)
            {
                Move();
            }
        }

        private void Move()
        {
    
            if (_humanoid != null && _humanoid.IsLife())
            {
                
                if (_agent.isOnNavMesh)
                    {
                        _agent.SetDestination(_humanoidPosition);
                        _isHumanoidInstalled = true;
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
            
            _agent.isStopped = true;
            _animator.SetBool(_enemyAnimController.Walk, false);
            StateMachine.EnterBehavior<EnemySearchTargetState>();
        }
        
        private IEnumerator CheckDistance()
        {
            while (_isHumanoidInstalled &&_humanoid.IsLife())
            {
                _distance = Vector3.Distance(transform.position, _humanoid.transform.position);

                if (_stoppingDistance >= _distance)
                {
                    _animator.SetBool(_enemyAnimController.Walk, false);
                    StateMachine.EnterBehavior<EnemyAttackState>();
                }

                if (!_humanoid.IsLife())
                {
                    _animator.SetBool(_enemyAnimController.Walk, false);
                    StateMachine.EnterBehavior<EnemySearchTargetState>();
                }
               
           
                yield return new WaitForSeconds(0.5f);
            }
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
               _agent.SetDestination(soldierPosition);
               
               yield return new WaitForSeconds(1.5f);
           }
           
           _agent.isStopped = false;
           _animator.SetBool(_enemyAnimController.Walk, true);
       }

       private bool ShouldTrackSoldier() {     return Random.value <= _trackingProbability; }

        public void SetHumanoidInstalled(bool isHumanoidInstalled)
       {
           _isHumanoidInstalled = isHumanoidInstalled;
       }
        
        
        
    }
}
// private void SetAnimInfo()
// {
//     foreach (KeyValuePair<int, float> info in _enemyAnimController.GetAnimInfo())
//     {
//         _animInfo.Add(info.Key, info.Value);
//         _stoppingTime = _animInfo[_enemyAnimController.Walk];
//     }
// }
// private async void StopRandomly()
// {
//     int minTime = 6;
//     int maxTime = 15;
//     int sec = 1000;
//     
//     
//     
//     while (isActiveAndEnabled)
//     {
//         await Task.Delay(Random.Range(minTime, maxTime) * sec);
//
//         if (!_isStopping)
//         {
//             _isStopping = true;
//             StopMovement();
//            // PlayScream();
//             
//             await Task.Delay(TimeSpan.FromSeconds(_stoppingTime));
//             _isStopping = false;
//             ResumeMovement();
//         }
//     }
// }

// private void PlayScream()
// {
//     AnimatorClipInfo[] currentClipInfo = _animator.GetCurrentAnimatorClipInfo(0);
//     AnimationClip currentClip = currentClipInfo.Length > 0 ? currentClipInfo[0].clip : null;
//     int randomIndex = Random.Range(0, _enemyAnimController.GetScreamAnimationClips().Length);
//     AnimationClip newClip = _enemyAnimController.GetScreamAnimationClips()[randomIndex];
//      _stoppingTime = newClip.length;
//      _enemyAnimController.SetRandomAnimation();
//     _animator.CrossFade(newClip.name, 0.2f);
// }
        
// private void StopMovement()
// {
//     _agent.isStopped = true;
//     _animator.SetBool("Walk", false); 
// }
//
// private void ResumeMovement()
// {
//     _agent.isStopped = false;
//     _animator.SetBool("Walk", true); 
// }
