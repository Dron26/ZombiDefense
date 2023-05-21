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
        private PlayerCharacterAnimController _playerCharacterAnimController;
        private Coroutine _coroutine;
        private FXController _fxController;
        private Enemy _enemy;
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
            _fxController = GetComponent<FXController>();
            _enemy=GetComponent<Enemy>();
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
                
                
                    if (_humanoid == null)
                        StopCoroutine(_coroutine);
                    else if ( _humanoid.IsLife() == false)
                    {
                        _humanoid.gameObject.SetActive(false);
                        StateMachine.EnterBehavior<EnemySearchTargetState>();
                        break;
                    }

                    if(_humanoid!=null)
                    {
                        _currentRange = Vector3.Distance(transform.position, _humanoid.transform.position);

                        if (_currentRange <= _enemy.GetRangeAttack())
                        {
                            _animator.SetTrigger(_playerCharacterAnimController.Attack);
                            transform.DOLookAt(_humanoid.transform.position, .1f);
                            _humanoid.ApplyDamage(_enemy.GetDamage());
                        }

                        if (_currentRange >= _enemy.GetRangeAttack())
                        {
                            _isAttack = false;
                            StateMachine.EnterBehavior<EnemyMovementState>();
                        }
                    

                    yield return _waitForSeconds;
                }
            }
        }
    }
}