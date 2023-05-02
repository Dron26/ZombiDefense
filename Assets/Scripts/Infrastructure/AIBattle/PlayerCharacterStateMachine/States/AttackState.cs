using System.Collections;
using DG.Tweening;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.Weapon;
using UnityEngine;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine.States
{
    public class AttackState : State
    {
        private readonly WaitForSeconds _waitForSeconds = new(1f);

        private Enemy _opponentEnemy;

        private float _currentRange;
        private bool _isAttack;

        private Animator _animator;
        private HashAnimator _hashAnimator;
        private Coroutine _coroutine;
        private FXController _fxController;
        private Humanoid _humanoid;
        private WeaponController _weaponController;
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _hashAnimator = GetComponent<HashAnimator>();
            _fxController = GetComponent<FXController>();
            _humanoid = GetComponent<Humanoid>();
            _weaponController= GetComponent<WeaponController>();
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

        public void InitEnemy(Enemy targetEnemy) =>
            _opponentEnemy = targetEnemy;

        private IEnumerator Attack()
        {
            _isAttack = true;

            while (_isAttack)
            {
                _animator.SetBool(_hashAnimator.IsShoot, true);
                if (_opponentEnemy == null)
                    StopCoroutine(_coroutine);

                if (_opponentEnemy != null && _opponentEnemy.IsLife() == false)
                {
                    _animator.SetBool(_hashAnimator.IsShoot, false);
                    PlayerCharactersStateMachine.EnterBehavior<SearchTargetState>();
                }

                if (_opponentEnemy != null)
                {
                    _currentRange = Vector3.Distance(transform.position, _opponentEnemy.transform.position);

                    if (_currentRange <= _weaponController.GetRangeAttack())
                    {
                        _fxController.OnAttackFX();
                        transform.DOLookAt(_opponentEnemy.transform.position, .1f);
                        _opponentEnemy.ApplyDamage(_weaponController.GetDamage());
                    }
                }


                if (_currentRange >= _weaponController.GetRangeAttack())
                {
                    _animator.SetBool(_hashAnimator.IsShoot, false);
                    _isAttack = false;
                    PlayerCharactersStateMachine.EnterBehavior<SearchTargetState>();
                }

                yield return _waitForSeconds;
            }
        }
    }
}