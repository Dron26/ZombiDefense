using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Infrastructure.AIBattle.EnemyAI.States
{
    public class EnemyMovementState : EnemyState
    {
        private readonly float _rateStepUnit = .01f;

        private Humanoid _humanoid;
        private NavMeshAgent agent;
        private float _stoppingDistance;
        private float _distance;
        private float _stoppingTime;
        private Animator _animator;
        private AnimController _animController;
        private Enemy _enemy;
        private bool _isStopping;
        private Dictionary<int, float> _animInfo=new();
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animController = GetComponent<AnimController>();
            agent = GetComponent<NavMeshAgent>();
            _enemy = GetComponent<Enemy>();
        }

        private void Start()
        {
            _stoppingDistance = _enemy.GetRangeAttack();
            _isStopping = false;
           // StopRandomly();
        }

        protected override void FixedUpdateCustom()
        {
            Move();
        }

        private async void StopRandomly()
        {
            int minTime = 6;
            int maxTime = 15;
            int sec = 1000;
            
            
            
            while (isActiveAndEnabled)
            {
                await Task.Delay(Random.Range(minTime, maxTime) * sec);

                if (!_isStopping)
                {
                    _isStopping = true;
                    StopMovement();
                    PlayScream();
                    
                    await Task.Delay(TimeSpan.FromSeconds(_stoppingTime));
                    _isStopping = false;
                    ResumeMovement();
                }
            }
        }

        private void PlayScream()
        {
            AnimatorClipInfo[] currentClipInfo = _animator.GetCurrentAnimatorClipInfo(0);
            AnimationClip currentClip = currentClipInfo.Length > 0 ? currentClipInfo[0].clip : null;
            int randomIndex = Random.Range(0, _animController.GetScreamAnimationClips().Length);
            AnimationClip newClip = _animController.GetScreamAnimationClips()[randomIndex];
             _stoppingTime = newClip.length;
            _animController.SetRandomAnimation();
            _animator.CrossFade(newClip.name, 0.2f);
        }
        
        private void StopMovement()
        {
            agent.isStopped = true;
            _animator.SetBool("Walk", false); 
        }

        private void ResumeMovement()
        {
            agent.isStopped = false;
            _animator.SetBool("Walk", true); 
        }
        
       public void InitHumanoid(Humanoid targetHumanoid) => 
            _humanoid = targetHumanoid;

       private void Move()
       {
           if (_humanoid != null)
           {
               Vector3 ourPosition = transform.position;
               Vector3 humanoidPosition;
               
               if ( _humanoid.IsLife())
               {
                   humanoidPosition = _humanoid.transform.position;
                   if (agent.isOnNavMesh)
                   {
                       agent.SetDestination(humanoidPosition);
                       Movement(ourPosition, humanoidPosition);
                   }
               }
               else
               {
                   ChangeState();
               }
           }
           else
           {
               ChangeState();
           }
           
       }


       private void ChangeState()
       {
           _animator.SetBool(_animController.Walk, false);
           StateMachine.EnterBehavior<EnemySearchTargetState>();
       }

       private void Movement(Vector3 ourPosition, Vector3 opponentPosition)
       {
           _animator.SetBool(_animController.Walk, true);
           
           // if (transform.position.y < -3.5)
           //     transform.position =
           //         new Vector3(ourPosition.x, ourPosition.y + 1.5f, ourPosition.z);

           // if (transform.rotation.x != 0) 
           //     transform.Rotate(0, ourPosition.y, ourPosition.z);
          //  
          // transform.position = new Vector3(MovementAxis(ourPosition.x, opponentPosition.x), 
          //     MovementAxis(ourPosition.y, opponentPosition.y), 
          //     MovementAxis(ourPosition.z, opponentPosition.z));
          //transform.DOLookAt(opponentPosition, .05f);
           
           TryNextState(ourPosition, opponentPosition);
       }

       private void TryNextState(Vector3 ourPosition, Vector3 opponentPosition)
       {
           _distance = Vector3.Distance(ourPosition, opponentPosition);

           if (_stoppingDistance >= _distance)
           {
               StateMachine.EnterBehavior<EnemyAttackState>();
           }
       }
       
       private void SetAnimInfo()
       {
           foreach (KeyValuePair<int, float> info in _animController.GetAnimInfo())
           {
               _animInfo.Add(info.Key, info.Value);
               _stoppingTime = _animInfo[_animController.Walk];
           }
       }
    }
}