using System;
using System.Collections;
using Animation;
using Characters.Humanoids.AbstractLevel;
using DG.Tweening;
using Enemies.AbstractEntity;
using UnityEngine;
using UnityEngine.XR;

namespace Infrastructure.AIBattle.EnemyAI.States
{
    public class EnemyAttackState : EnemyState
    {
        private readonly WaitForSeconds _waitForSeconds = new(1f);

        private Character _character;

        private float _currentRange;
        private bool _isAttack;

        private Animator _animator;
        private EnemyAnimController _enemyAnimController;
        private FXController _fxController;
        private Enemy _enemy;
        private bool _isAttacked;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _enemyAnimController = GetComponent<EnemyAnimController>();
            _fxController = GetComponent<FXController>();
            _enemy= GetComponent<Enemy>();
        }
        
            
        public override void OnTakeGranadeDamage()
        {
            _isAttack = false;
            _enemyAnimController.OnAttack(false);
            saveLoadService.OnSetActiveHumanoid-=OnSetActiveHumanoid;
            StateMachine.EnterBehavior<EnemyStunningState>();
        }
    

        private void Start()
        {
            saveLoadService.OnSetActiveHumanoid+=OnSetActiveHumanoid;
        }

        private void OnSetActiveHumanoid()
        {
            ChangeState();
        }

        protected override void UpdateCustom()
        {
            if (_isAttack == false)
            {
                _isAttack = true;
                StartCoroutine(Attack());
            }
        }

        public void InitCharacter(Character targetCharacter)
        {
            _character = targetCharacter;
            _isAttacked = false;
        }

        private IEnumerator Attack()
        {
            Vector3 ourPosition = transform.position;
            
            while (_isAttack&&_character.IsLife)
            {

                _currentRange = Vector3.Distance(transform.position, _character.transform.position);

                if (_currentRange <= _enemy.GetRangeAttack()&&_isAttacked==false)
                {
                    _isAttacked = true;
                    _enemyAnimController.OnAttack(true);
                    transform.DOLookAt(_character.transform.position, .1f);
                    _character.ApplyDamage(_enemy.GetDamage());
                }
              
                if (_currentRange >= _enemy.GetRangeAttack()||_character.IsLife==false)
                {
                    _isAttacked = false;
                    ChangeState();
                    break;
                }
                
                yield return new WaitForSeconds(_enemyAnimController._currentClip.length);
            }
            
            ChangeState();
            
            yield return null;
        }

        private void ChangeState()
        {
            _isAttacked = false;
            _isAttack = false;
            _enemyAnimController.OnAttack(false);
            saveLoadService.OnSetActiveHumanoid-=OnSetActiveHumanoid;
            StateMachine.EnterBehavior<EnemySearchTargetState>();
        }
        
        public void AttackEnd()
        {
            _isAttacked = false;
        }


        public override void Disable()
        {
            saveLoadService.OnSetActiveHumanoid-=OnSetActiveHumanoid;
        }
    }
}