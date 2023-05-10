using System;
using DG.Tweening;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.EnemyAI.States
{
    public class EnemyMovementState : EnemyState
    {
        private readonly float _rateStepUnit = .01f;

        private Humanoid _humanoid;
        private NavMeshAgent agent;
        private float _stoppingDistance;
        private float _distance;
        private Animator _animator;
        private HashAnimator _hashAnimator;
        private Enemy _enemy;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _hashAnimator = GetComponent<HashAnimator>();
            agent = GetComponent<NavMeshAgent>();
            _enemy = GetComponent<Enemy>();
        }

        private void Start()
        {
            _stoppingDistance = _enemy.GetRangeAttack();
        }

        protected override void FixedUpdateCustom()
        {
            Move();
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
           _animator.SetBool(_hashAnimator.Walk, false);
           StateMachine.EnterBehavior<EnemySearchTargetState>();
       }

       private void Movement(Vector3 ourPosition, Vector3 opponentPosition)
       {
           _animator.SetBool(_hashAnimator.Walk, true);
           
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
    }
}