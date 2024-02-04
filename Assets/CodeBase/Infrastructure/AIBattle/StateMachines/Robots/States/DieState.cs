using System.Collections;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.StateMachines.Robots.States
{
    public class DieState: State
        {
            public delegate void RobotDeathHandler(Characters.Robots.Robot robot);
            public event RobotDeathHandler OnDeath;
            private Characters.Robots.Robot _robot;
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
                _robot=GetComponent<Characters.Robots.Robot>();
                OnDeath?.Invoke(_robot);
            
                _robot.GetComponent<Rigidbody>().useGravity=false;
                _robot.GetComponent<Collider>().enabled = false;
                _agent.enabled = false;
                yield return  _wait;
            
                StartCoroutine(Fall());
                yield return  _wait;
            
                _robot.gameObject.SetActive(false);
                _robot.gameObject.transform.position = _robot.StartPosition;
            
                Destroy(gameObject);
            
                yield break;
            }
            private  IEnumerator Fall()
            {
                while (isActiveAndEnabled!=false)
                {
                    float newPosition=_robot.transform.position.y-0.0001f;
                    _robot.transform.position=new Vector3(_robot.transform.position.x,newPosition,_robot.transform.position.z);
                    yield return null;
                }
            
                yield break;
            }

            public override void ExitBehavior()
            {
            }
        }
    }