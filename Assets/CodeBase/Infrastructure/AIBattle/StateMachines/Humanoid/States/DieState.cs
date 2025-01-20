using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.StateMachines.Humanoid.States
{
    public class DieState : State
    {
        public delegate void HumanoidDeathHandler(Characters.Humanoids.AbstractLevel.Humanoid humanoid);
        public event HumanoidDeathHandler OnDeath;
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
            _humanoid.GetComponent<Rigidbody>().useGravity=false;
            _humanoid.GetComponent<Collider>().enabled = false;
            _agent.enabled = false;
            yield return  _wait;
            
            StartCoroutine(Fall());
            yield return  _wait;

            if (_humanoid!=null)
            {
                _humanoid.gameObject.SetActive(false);
                _humanoid.gameObject.transform.position = _humanoid.StartPosition;
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
                    float newPosition=_humanoid.transform.position.y-0.0001f;
                    _humanoid.transform.position=new Vector3(_humanoid.transform.position.x,newPosition,_humanoid.transform.position.z);
                }
                yield return null;
            }
        }

        public override void ExitBehavior()
        {
        }
    }
}