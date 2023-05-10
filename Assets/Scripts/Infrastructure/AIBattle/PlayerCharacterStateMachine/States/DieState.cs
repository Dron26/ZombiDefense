using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.System;
using Infrastructure.WaveManagment;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.EnemyAI.States
{
    public class DieState : State
    {
        public delegate void HumanoidDeathHandler(Humanoid humanoid);
        public event HumanoidDeathHandler OnDeath;
        private Humanoid _humanoid;
        private void Start()
        {
            StartCoroutine(WaitAfterDie());
        }
        private  IEnumerator WaitAfterDie()
        {
            
            _humanoid=GetComponent<Humanoid>();
            OnDeath?.Invoke(_humanoid);
            
            _humanoid.GetComponent<Rigidbody>().useGravity=false;
            _humanoid.GetComponent<Collider>().enabled = false;
            yield return  new WaitForSeconds(4f);
            
            StartCoroutine(Fall());
            yield return  new WaitForSeconds(6f);
            
            _humanoid.gameObject.SetActive(false);
            _humanoid.gameObject.transform.position = _humanoid.StartPosition;
            
            yield break;
        }
        private  IEnumerator Fall()
        {
            while (isActiveAndEnabled!=false)
            {
                float newPosition=_humanoid.transform.position.y-0.0001f;
                _humanoid.transform.position=new Vector3(_humanoid.transform.position.x,newPosition,_humanoid.transform.position.z);
                yield return null;
            }
            
            yield break;
        }
    }
}