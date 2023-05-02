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

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _hashAnimator = GetComponent<HashAnimator>();
            agent = GetComponent<NavMeshAgent>();

            if (TryGetComponent(out Enemy enemy)) 
                _stoppingDistance = enemy.GetRangeAttack();
        }

        protected override void FixedUpdateCustom()
        {
            if (isActiveAndEnabled == false)
                return;

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
                   agent.SetDestination(humanoidPosition);
                   Movement(ourPosition, humanoidPosition);
               }
               else
               {
                   _animator.SetBool(_hashAnimator.IsRun, false);
                   StateMachine.EnterBehavior<EnemySearchTargetState>();
               }
           }

           print(gameObject.name);
           print("EnemyMovementState  Take Null humanoid");
       }


       private void Movement(Vector3 ourPosition, Vector3 opponentPosition)
       {
           _animator.SetBool(_hashAnimator.IsRun, true);
           
           // if (transform.position.y < -3.5)
           //     transform.position =
           //         new Vector3(ourPosition.x, ourPosition.y + 1.5f, ourPosition.z);

           // if (transform.rotation.x != 0) 
           //     transform.Rotate(0, ourPosition.y, ourPosition.z);
          //  
          // transform.position = new Vector3(MovementAxis(ourPosition.x, opponentPosition.x), 
          //     MovementAxis(ourPosition.y, opponentPosition.y), 
          //     MovementAxis(ourPosition.z, opponentPosition.z));

         //  transform.DOLookAt(opponentPosition, .05f);
           
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