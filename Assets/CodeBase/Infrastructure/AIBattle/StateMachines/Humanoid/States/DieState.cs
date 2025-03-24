using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.StateMachines.Humanoid.States
{
    public class DieState : State
    {
        public delegate void HumanoidDeathHandler(Characters.Humanoids.AbstractLevel.Humanoid humanoid);
        private Characters.Humanoids.AbstractLevel.Humanoid _humanoid;
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
            
            _humanoid=GetComponent<Characters.Humanoids.AbstractLevel.Humanoid>();
            GetComponent<Rigidbody>().useGravity=false;
            GetComponent<Collider>().enabled = false;
            _agent.enabled = false;
            yield return  _wait;
            
            StartCoroutine(Fall());
            yield return  _wait;

            if (_humanoid!=null)
            {
                gameObject.SetActive(false);
                transform.position = _humanoid.StartPosition;
                Destroy(gameObject);
            }
            yield break;
        }
        private  IEnumerator Fall()
        {
            while (isActiveAndEnabled!=false)
            {
                if (_humanoid!=null)
                {
                    var position = transform.position;
                    float newPosition=position.y-0.0001f;
                    transform.position=new Vector3(position.x,newPosition,position.z);
                    
                }
                yield return null;
            }
        }

        public override void ExitBehavior()
        {
        }
    }
}