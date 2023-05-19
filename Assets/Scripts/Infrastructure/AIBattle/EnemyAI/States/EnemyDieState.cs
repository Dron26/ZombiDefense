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
        private void FixedUpdate()
        {
            if (!_isDeath)
            {
                StopCoroutine(WaitBeforeDie());
                StartCoroutine(WaitBeforeDie());
            }
        }
        
        private  IEnumerator WaitBeforeDie()
        {
            _isDeath=true;

            _enemy=GetComponent<Enemy>();
            
            if (_enemy.Level == 2)
            {
                FXController _fxController = GetComponent<FXController>();
                _fxController.OnTankDeathFX();
                yield return  new WaitForSeconds(1f);
                _enemy.gameObject.SetActive(false);
                _enemy.gameObject.transform.position = _enemy.StartPosition;
            }
            
            _enemy.GetComponent<Rigidbody>().useGravity=false;
            _enemy.GetComponent<Collider>().enabled = false;
            _enemy.GetComponent<NavMeshAgent>().enabled = false;
            
            yield return  new WaitForSeconds(2f);
            
            StartCoroutine(Fall());
            yield return  new WaitForSeconds(2f);
            _enemy.gameObject.SetActive(false);
            _enemy.gameObject.transform.position = _enemy.StartPosition;

            
            AfterDie();
        }
        
        private void AfterDie()
        {
            _enemy.GetComponent<Rigidbody>().useGravity=true;
            _enemy.GetComponent<Collider>().enabled = true;
            _enemy.GetComponent<NavMeshAgent>().enabled = true;
            
            OnRevival?.Invoke(_enemy);
            StateMachine.EnterBehavior<EnemySearchTargetState>();
            _isDeath=false;
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