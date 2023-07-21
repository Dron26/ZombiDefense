using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.System;
using Infrastructure.FactoryWarriors.Humanoids;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.EnemyAI.States
{
    public class EnemyDieState : EnemyState
    {
        public delegate void EnemyRevivalHandler(Enemy enemy);
        public event EnemyRevivalHandler OnRevival;
        private Enemy _enemy;
        private bool _isDeath;
        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
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
            _enemy=GetComponent<Enemy>();
            _enemy.GetComponent<NavMeshAgent>().enabled = false;
            _isDeath=true;
            
            if (_enemy.Level == 2)
            {
                FXController _fxController = GetComponent<FXController>();
                _fxController.OnTankDeathFX();
                yield return  new WaitForSeconds(1f);
                _enemy.gameObject.SetActive(false);
                _enemy.gameObject.transform.position = _enemy.StartPosition;
            }
            
            _enemy.GetComponent<Collider>().enabled = false;
            _enemy.GetComponent<NavMeshAgent>().enabled = false;
            
            yield return  new WaitForSeconds(4f);
            
            StartCoroutine(Fall());
            yield return  new WaitForSeconds(2f);
            _enemy.gameObject.SetActive(false);
            _enemy.gameObject.transform.position = _enemy.StartPosition;

            
            AfterDie();
            yield break;
        }
        
        private void AfterDie()
        {
            _enemy.GetComponent<Collider>().enabled = true;
            _enemy.GetComponent<NavMeshAgent>().enabled = true;
            
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
        
    }
}