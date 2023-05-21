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
        private NavMeshAgent _agent;
        private float _move;
        private bool isStopped = false;
        private Animator _animator;
        private PlayerCharacterAnimController _playerCharacterAnimController;
        private WeaponController _weaponController;
        private float minDistance = 0.4f;
        private bool reachedDestination = true;
        private bool isSetDestination = false;

        private void Awake()
        {
            _weaponController = GetComponent<WeaponController>();
            _animator = GetComponent<Animator>();
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
            _agent = GetComponent<NavMeshAgent>();
            _agent.stoppingDistance = 0f; // Задайте минимальную дистанцию остановки
            _move = 0f;
        }


        protected override void FixedUpdateCustom()
        {
            if (!isActiveAndEnabled)
                return;

            if (reachedDestination)
            {
                if (!isSetDestination)
                {
                    Move();
                }

                CheckDistance();
            }
        }

        private void Move()
        {
            _point = PlayerCharactersStateMachine.GetPoint();

            if (_point != null)
            {
                Vector3 opponentPosition = _point.transform.position;
                _animator.SetBool(_playerCharacterAnimController.Run, true);
                _agent.SetDestination(opponentPosition);
                isSetDestination = true;
            }
            else
            {
                print("Invalid point");
            }
        }


        private void CheckDistance()
        {
            if (_point == null)
                return;

            float distance = Vector3.Distance(transform.position, _point.transform.position);
            if (distance <= minDistance)
            {
                _animator.SetBool(_playerCharacterAnimController.Run, false);
                reachedDestination = true;
                isSetDestination = false;
                Humanoid humanoid =GetComponent<Humanoid>();
                _point.SetHumanoid(humanoid);
                PlayerCharactersStateMachine.EnterBehavior<SearchTargetState>();
            }
        }
    }
}