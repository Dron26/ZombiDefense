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
        public delegate void EnemyDeathHandler(Enemy enemy);
        public event EnemyDeathHandler OnDeath;
        private Enemy _enemy;
        private void Start()
        {
            StartCoroutine(WaitAfterDie());
        }
        private  IEnumerator WaitAfterDie()
        {
            

            _enemy=GetComponent<Enemy>();
            
            if (_enemy.Level == 4)
            {
                FXController _fxController = GetComponent<FXController>();
                _fxController.OnTankDeathFX();
                yield return  new WaitForSeconds(1f);
                _enemy.gameObject.SetActive(false);
                _enemy.gameObject.transform.position = _enemy.StartPosition;
            }
            OnDeath?.Invoke(_enemy);
            
            _enemy.GetComponent<Rigidbody>().useGravity=false;
            _enemy.GetComponent<Collider>().enabled = false;
            _enemy.GetComponent<NavMeshAgent>().enabled = false;
            
            yield return  new WaitForSeconds(4f);

            
            
            StartCoroutine(Fall());
            yield return  new WaitForSeconds(6f);
            _enemy.gameObject.SetActive(false);
            _enemy.gameObject.transform.position = _enemy.StartPosition;
            
            yield break;
        }
        private  IEnumerator Fall()
        {
            while (isActiveAndEnabled!=false)
            {
                float newPosition=_enemy.transform.position.y-0.0001f;
                _enemy.transform.position=new Vector3(_enemy.transform.position.x,newPosition,_enemy.transform.position.z);
                yield return null;
            }
            
            yield break;
        }
        
    }
}