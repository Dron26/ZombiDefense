using System.Collections;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.StateMachines.Robots.States
{
    public class DieState: State
        {
            public delegate void RobotDeathHandler(Characters.Robots.Turret turret);
            public event RobotDeathHandler OnDeath;
            private Characters.Robots.Turret _turret;
            private WaitForSeconds _wait;
            private float _waitTime=5f;
            private NavMeshAgent _agent;

            private void Start()
            {
                _wait = new WaitForSeconds(_waitTime);
                _agent = GetComponent<NavMeshAgent>();
                StartCoroutine(WaitAfterDie());
            }
            private  IEnumerator WaitAfterDie()
            {
                _turret=GetComponent<Characters.Robots.Turret>();
                OnDeath?.Invoke(_turret);
            
                _turret.GetComponent<Rigidbody>().useGravity=false;
                _turret.GetComponent<Collider>().enabled = false;
                _agent.enabled = false;
                yield return  _wait;
            
                StartCoroutine(Fall());
                yield return  _wait;
            
                _turret.gameObject.SetActive(false);
                _turret.gameObject.transform.position = _turret.StartPosition;
            
                Destroy(gameObject);
            
                yield break;
            }
            private  IEnumerator Fall()
            {
                while (isActiveAndEnabled!=false)
                {
                    float newPosition=_turret.transform.position.y-0.0001f;
                    _turret.transform.position=new Vector3(_turret.transform.position.x,newPosition,_turret.transform.position.z);
                    yield return null;
                }
            
                yield break;
            }

            public override void ExitBehavior()
            {
            }
        }
    }