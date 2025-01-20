using System.Collections;
using Animation;
using Characters.Humanoids.AbstractLevel;
using DG.Tweening;
using Enemies;
using Enemies.AbstractEntity;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Infrastructure.AIBattle.StateMachines.EnemyAI.States
{
    public class EnemyAttackState : EnemyState
    {
        private WaitForSeconds _waitForSeconds = new(1f);
        private WaitForSeconds _waitForSecondsTwo = new(2f);

        private Character _character;
        private float _currentRange;
        private float _throwerRange;
        private float _rangeHit;
        private bool _isAttack;
        private Animator _animator;
        private EnemyAnimController _enemyAnimController;
        private EnemyFXController _fxController;
        private Enemy _enemy;
        private bool _isAttacked;
        private bool _isThrower;
        private bool _isThrowering;

        private Coroutine _attackCoroutine;

        protected override void OnInitialized()
        {
            _animator = StateMachine.Enemy.Animator;
            _enemyAnimController = StateMachine.Enemy.EnemyAnimController;
            _fxController = StateMachine.Enemy.FXController;
            _enemy = StateMachine.Enemy;
        }

        protected override void OnEnter()
        {
            enabled = true;
            _isThrower = _enemy.Data.IsThrower;
            _rangeHit = _enemy.GetRangeAttack();

            if (_isThrower)
            {
                _throwerRange = _enemy.GetThrowerRangeAttack();
            }

            _isAttack = false;
            _isAttacked = false;
            _isThrowering = false;

            // Запуск корутины атаки
            _attackCoroutine = StartCoroutine(AttackRoutine());
        }

        protected override void OnExit()
        {
            _isAttack = false;
            _isAttacked = false;
            _isThrowering = false;

            _enemyAnimController.OnAttack(false);

            // Остановка корутины атаки
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }

            if (_character != null)
                StopCoroutine(_fxController.OnThrowFlesh(_character.transform.position));

            enabled = false;
        }

        public void InitCharacter(Character targetCharacter)
        {
            _character = targetCharacter;
        }

        private IEnumerator AttackRoutine()
        {
            
            yield return null;
            
            while (_enemy.IsLife() && _character.IsLife())
            {
                _currentRange = Vector3.Distance(transform.position, _character.transform.position);

                if (_currentRange <= _rangeHit && !_isAttacked)
                {
                    PerformMeleeAttack();
                }
                else if (_isThrower && _currentRange > _rangeHit && _currentRange > _throwerRange / 3 && _currentRange < _throwerRange)
                {
                    PerformThrow();
                    yield break;
                }
                else if(_isAttacked)
                {
                    yield return _waitForSeconds;
                }

                if (_isThrowering)
                    yield return _waitForSecondsTwo;
                yield return null;
            }

            ChangeState();
        }

        private void PerformThrow()
        {
            StateMachine.EnterBehavior<EnemyThrowState>();
            _isThrowering = true;
        }

        private void PerformMeleeAttack()
        {
            if (_enemy.Data.Type == EnemyType.Smoker)
            {
                _character.ApplyDamage(_enemy.Data.ExplosiveAbility.ExplosiveDamage);
                _enemy.ApplyDamage(_enemy.Data.ExplosiveAbility.ExplosiveDamage, ItemType.EnemyExplosion);
                ChangeState();
            }
            else
            {
                Attack();
            }
        }

        private void SetHit()
        {
            Debug.Log("AttackEnemy()");
            
            _character.ApplyDamage(_enemy.GetDamage());
        }
        
        private void Attack()
        {
            _isAttacked = true;
            transform.DOLookAt(_character.transform.position, .1f);
            _enemyAnimController.OnAttack(true);
        }
        
        private void AttackEnd()
        {
            _isAttacked = false;
        }

        private void ChangeState()
        {
            StateMachine.EnterBehavior<EnemySearchTargetState>();
        }

        public override void Disable()
        {
            // Очистка ресурсов, если требуется
        }
    }
}