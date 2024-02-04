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

        private Humanoid _humanoid;

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

        public void InitHumanoid(Humanoid targetHumanoid)
        {
            _humanoid = targetHumanoid;
        }

        private IEnumerator Attack()
        {
            Vector3 ourPosition = transform.position;
            
            while (_isAttack&&_humanoid.IsLife)
            {

                _currentRange = Vector3.Distance(transform.position, _humanoid.transform.position);

                if (_currentRange <= _enemy.GetRangeAttack()&&_isAttacked==false)
                {
                    _isAttacked = true;
                    _enemyAnimController.OnAttack(true);
                    transform.DOLookAt(_humanoid.transform.position, .1f);
                    _humanoid.ApplyDamage(_enemy.GetDamage());
                }

                if (_currentRange >= _enemy.GetRangeAttack()||_humanoid.IsLife==false)
                {
                    ChangeState();
                }
                
                yield return _waitForSeconds;
            }
            
            ChangeState();
        }

        private void ChangeState()
        {
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