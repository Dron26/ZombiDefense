using System.Collections;
using Characters.Humanoids.AbstractLevel;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine.States
{
    public class DieState : State
    {
        public delegate void HumanoidDeathHandler(Humanoid humanoid);
        public event HumanoidDeathHandler OnDeath;
        private Humanoid _humanoid;
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
            _humanoid=GetComponent<Humanoid>();
            OnDeath?.Invoke(_humanoid);
            
            _humanoid.GetComponent<Rigidbody>().useGravity=false;
            _humanoid.GetComponent<Collider>().enabled = false;
            _agent.enabled = false;
            yield return  _wait;
            
            StartCoroutine(Fall());
            yield return  _wait;
            
            _humanoid.gameObject.SetActive(false);
            _humanoid.gameObject.transform.position = _humanoid.StartPosition;
            
            Destroy(gameObject);
            
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

        public override void ExitBehavior()
        {
        }
    }
}