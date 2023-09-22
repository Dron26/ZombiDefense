using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.System;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.EnemyAI.States
{
    public class EnemyDieState : EnemyState
    {
        public delegate void EnemyRevivalHandler(Enemy enemy);
        public event EnemyRevivalHandler OnRevival;
        private Enemy _enemy;
        private NavMeshAgent _agent;
        private Collider _collider;
        private bool _isDeath;
        private FXController _fxController;
        private WaitForSeconds _wait;
        private bool _isInfinityOn = false;
        private int _waitTime=1;
        private void Start()
        {
            _enemy=GetComponent<Enemy>();
            _agent = GetComponent<NavMeshAgent>();
            _collider=GetComponent<Collider>();
            _fxController = GetComponent<FXController>();
            _wait = new WaitForSeconds(_waitTime);
        }

        protected override void FixedUpdateCustom()
        {
            if (!_isDeath)
            {
                _agent.isStopped = true;
                StartCoroutine(WaitBeforeDie());
            }
        }
        
        private  IEnumerator WaitBeforeDie()
        {
            _waitTime=1;
            _agent.enabled = false;
            _isDeath=true;
            
            if (_enemy.Level == 2)
            {
                _fxController.OnTankDeathFX();
                yield return  _wait;
                _enemy.gameObject.SetActive(false);
                _enemy.gameObject.transform.position = _enemy.StartPosition;
            }
            
            _collider.enabled = false;
            _waitTime = 4;
            yield return  _wait;
            
            StartCoroutine(Fall());
            
            _waitTime = 2;
            yield return  _waitTime;
            
            _enemy.gameObject.SetActive(false);
            _enemy.gameObject.transform.position = _enemy.StartPosition;

            if (_isInfinityOn)
            {
                
                    AfterDie();

            }
            
            yield break;
        }
        
        public void AfterDie()
        {
            _collider.enabled = true;
            _agent.enabled = true;
            
            OnRevival?.Invoke(_enemy);
            _isDeath=false;
            _agent.isStopped = false;
            StateMachine.EnterBehavior<EnemySearchTargetState>();
        }
        
        
        private  IEnumerator Fall()
        {
            while (isActiveAndEnabled!=false)
            {
                float newPosition=_enemy.transform.position.y-0.008f;
                _enemy.transform.position=new Vector3(_enemy.transform.position.x,newPosition,_enemy.transform.position.z);
                yield return null;
            }
            
            yield break;    
        }

        public void SetInfinity()
        {
            _isInfinityOn = true;
        }
    }
}