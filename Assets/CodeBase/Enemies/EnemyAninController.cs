using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EnemyAninController : MonoBehaviour
    {
    
        private NavMeshAgent agent;
        private Animator _animator;

        private float _move;
        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            _move = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            _animator.SetFloat("Speed", _move);
            agent.velocity=_animator.deltaPosition/Time.deltaTime;
        }
    }
}