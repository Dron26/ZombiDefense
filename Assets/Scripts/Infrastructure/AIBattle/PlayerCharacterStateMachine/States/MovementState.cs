using System;
using System.Collections.Generic;
using DG.Tweening;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.Location;
using Infrastructure.WeaponManagment;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine.States
{
    public class MovementState : State
    {
        private readonly float _rateStepUnit = .01f;

        private WorkPoint _point;
        private Enemy _opponentEnemy;
        private NavMeshAgent _agent;
        private float _stoppingDistance;
        private float _distance;
        private float _move;
        private bool isStopped = false;
        private Animator _animator;
        private AnimController _animController;
        private WeaponController _weaponController;

        private void Awake()
        {
            _weaponController = GetComponent<WeaponController>();
            _animator = GetComponent<Animator>();
            _animController = GetComponent<AnimController>();
            _stoppingDistance = _weaponController.GetRangeAttack();
            _agent = GetComponent<NavMeshAgent>();
            _move = 0f;
        }


        protected override void FixedUpdateCustom()
        {
            if (isActiveAndEnabled == false)
                return;

            Move();
        }

        private void Move()
        {
            _point = PlayerCharactersStateMachine.GetPoint();
            
            if (_point!=null)
            {
                Vector3 ourPosition = transform.position;
                Vector3 opponentPosition;

                opponentPosition = _point.transform.position;
                _agent.SetDestination(opponentPosition);
                Movement(ourPosition, opponentPosition);
            }
            else
            {
               print("Invalid point");
            }
        }

        private void Movement(Vector3 ourPosition, Vector3 pointPosition)
        {
            _animator.SetBool(_animController.Run, true);

            //  if (transform.position.y < -3.5)
            //      transform.position =
            //          new Vector3(ourPosition.x, ourPosition.y + 1.5f, ourPosition.z);
            //
            //  if (transform.rotation.x != 0) 
            //      transform.Rotate(0, ourPosition.y, ourPosition.z);
            //  
            transform.position = new Vector3(MovementAxis(ourPosition.x, pointPosition.x),
                MovementAxis(ourPosition.y, pointPosition.y),
                MovementAxis(ourPosition.z, pointPosition.z));

            transform.DOLookAt(pointPosition, .05f);

            TryNextState(ourPosition, pointPosition);
        }

        private void TryNextState(Vector3 ourPosition, Vector3 pointPosition)
        {
            if (ourPosition == pointPosition)
            {
                PlayerCharactersStateMachine.EnterBehavior<SearchTargetState>();
            }
        }

        private float MovementAxis(float ourPosition, float targetPosition) =>
            Mathf.MoveTowards(ourPosition, targetPosition, _rateStepUnit);
    }
}