using System.Collections;
using DG.Tweening;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using UnityEngine;

namespace Infrastructure.AIBattle.EnemyAI.States
{
    public class EnemyAttackState : EnemyState
    {
        private readonly WaitForSeconds _waitForSeconds = new (1f);
        
        private Humanoid _humanoid;

        private float _currentRange;
        private bool _isAttack;
        
        private Animator _animator;
        private HashAnimator _hashAnimator;
        private Coroutine _coroutine;
        private FXController _fxController;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _hashAnimator = GetComponent<HashAnimator>();
            _fxController = GetComponent<FXController>();
        }

        protected override void UpdateCustom()
        {
            if (isActiveAndEnabled == false)
            {
                if (_coroutine != null) 
                    StopCoroutine(_coroutine);
                
                return;
            }

            _coroutine ??= StartCoroutine(Attack());
        }

        public void InitHumanoid(Humanoid targetHumanoid)
        {
            
            _humanoid = targetHumanoid;
        }

        private IEnumerator Attack()
        {
            Vector3 ourPosition = transform.position;
            _isAttack = true;

            while (_isAttack)
            {
                if (transform.position.y < -3.5)
                    transform.position =
                        new Vector3(ourPosition.x, ourPosition.y + 1.5f, ourPosition.z);
                
                _animator.SetBool(_hashAnimator.IsShoot, true);
                
                if (TryGetComponent(out Enemy enemy))
                {
                    if (_humanoid == null)
                        StopCoroutine(_coroutine);

                    if (_humanoid != null && _humanoid.IsLife() == false)
                    {
                        _animator.SetBool(_hashAnimator.IsShoot, false);
                        _humanoid.gameObject.SetActive(false);
                        StateMachine.EnterBehavior<EnemySearchTargetState>();
                    }

                    if(_humanoid!=null){
                        
                        _currentRange = Vector3.Distance(transform.position, _humanoid.transform.position);

                        if (_currentRange <= enemy.GetRangeAttack())
                        {
                            _fxController.OnAttackFX();
                            transform.DOLookAt(_humanoid.transform.position, .1f);
                            _humanoid.ApplyDamage(enemy.GetDamage());
                        }

                        if (_currentRange >= enemy.GetRangeAttack())
                        {
                            _animator.SetBool(_hashAnimator.IsShoot, false);
                            _isAttack = false;
                            StateMachine.EnterBehavior<EnemyMovementState>();
                        }}
                    

                    yield return _waitForSeconds;
                }
            }
        }
    }
}