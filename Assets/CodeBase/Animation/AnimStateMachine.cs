using UnityEngine;

namespace Animation
{
    public class AnimStateMachine:StateMachineBehaviour
    {       

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("OnStateEnter: " + stateInfo.shortNameHash);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") )
            {
                
            }
            Debug.Log("OnStateExit: "+ stateInfo.shortNameHash);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("Update");
        }
    }
}