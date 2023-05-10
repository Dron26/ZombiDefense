using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.WeaponManagment;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine.States
{
    public class MovementState : State
    {
        private readonly float _rateStepUnit = .01f;

        private Humanoid _opponentHumanoid;
        private Enemy _opponentEnemy;
        private NavMeshAgent agent;
        private float _stoppingDistance;
        private float _distance;
        private float _move;
        
        private Animator _animator;
        private HashAnimator _hashAnimator;
        private WeaponController _weaponController;
        
        private void Start()
        {
            _weaponController= GetComponent<WeaponController>();
            _move = 0f;
            _animator = GetComponent<Animator>();
            _hashAnimator = GetComponent<HashAnimator>();

            if (TryGetComponent(out Enemy enemy)) 
                _stoppingDistance = enemy.GetRangeAttack();
            
            if (TryGetComponent(out Humanoid humanoid)) 
                _stoppingDistance = _weaponController.GetRangeAttack();
            agent = GetComponent<NavMeshAgent>();
        }

       protected override void FixedUpdateCustom()
       {
           if (isActiveAndEnabled == false)
               return;
           
                     Move();
       }

       public void InitHumanoid(Humanoid targetHumanoid) => 
            _opponentHumanoid = targetHumanoid;

       public void InitEnemy(Enemy targetEnemy) =>
            _opponentEnemy = targetEnemy;

       private void Move()
       {
           if (_opponentHumanoid != null && _opponentHumanoid.IsLife() == false
               || _opponentEnemy != null && _opponentEnemy.IsLife() == false)
           {
               //_animator.SetBool(_hashAnimator.Run, false);
               PlayerCharactersStateMachine.EnterBehavior<SearchTargetState>();
           }
           
           if (_opponentHumanoid != null && _opponentEnemy != null)
           {_move = 0.2f;
              
               PlayerCharactersStateMachine.EnterBehavior<SearchTargetState>();
           }
           
           Vector3 ourPosition = transform.position;
           Vector3 opponentPosition;
           
           if (_opponentHumanoid != null && _opponentHumanoid.IsLife())
           {
               opponentPosition = _opponentHumanoid.transform.position;
               agent.SetDestination(opponentPosition);
                Movement(ourPosition, opponentPosition);
           }
           
           if (_opponentHumanoid == null)
           {
               _animator.SetBool(_hashAnimator.Run, false);
              // Movement(ourPosition, opponentPosition);
           }
       }

       private void Movement(Vector3 ourPosition, Vector3 opponentPosition)
       {
           _animator.SetBool(_hashAnimator.Run, true);
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
               PlayerCharactersStateMachine.EnterBehavior<AttackState>();
           }
       }

       private float MovementAxis(float ourPosition, float targetPosition) => 
            Mathf.MoveTowards(ourPosition, targetPosition, _rateStepUnit);
    }
}