using System.Collections;
using Humanoids.AbstractLevel;
using UnityEngine;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine.States
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