using System;
using System.Collections;
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
        private readonly WaitForSeconds _waitForSeconds = new(0.3f);
        private WorkPoint _point;
        private NavMeshAgent _agent;
        private float _move;
        private bool isStopped = false;
        private PlayerCharacterAnimController _playerCharacterAnimController;
        private WeaponController _weaponController;
        private float minDistance = 0.3f;
        private bool reachedDestination = true;
        private bool isSetDestination = false;
        private Humanoid _humanoid;
        
        private void Awake()
        {
            _humanoid=GetComponent<Humanoid>();
            _weaponController = GetComponent<WeaponController>();
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
            }
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
                isSetDestination = true;
                StartCoroutine(CheckDistance()) ;
            }
            else
            {
                print("Invalid point");
            }
        }


        private IEnumerator CheckDistance()
        {
            reachedDestination = false;
            if (_point == null)
                yield return null;

            while (reachedDestination==false)
            {
                float distance = Vector3.Distance(transform.position, _point.transform.position);
            
                if (distance <= minDistance)
                {
                    reachedDestination = true;
                    isSetDestination = false;
                    Humanoid humanoid =GetComponent<Humanoid>();
                    _point.SetHumanoid(humanoid);
                    _humanoid.IsMoving(false);
                    _playerCharacterAnimController.OnMove(false);
                    PlayerCharactersStateMachine.EnterBehavior<SearchTargetState>();
                }
                
                yield return _waitForSeconds;
            }
        }

        public void SetNewPoint(WorkPoint newPoint)
        {
            _point = newPoint;
            reachedDestination = true;
            isSetDestination = false;
        }
    }
}