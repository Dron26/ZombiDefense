using System.Collections;
using System.Collections.Generic;
using Animation;
using Characters.Humanoids.AbstractLevel;
using Enemies;
using Enemies.AbstractEntity;
using Infrastructure.Logic.WeaponManagment;
using Interface;
using Services;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.StateMachines.EnemyAI.States
{
    public class EnemyDieState : EnemyState
    {
        public delegate void EnemyRevivalHandler(Enemy enemy);
        public event EnemyRevivalHandler OnRevival;

        private Enemy _enemy;
        private NavMeshAgent _agent;
        private Collider _collider;
        private EnemyFXController _fxController;
        private EnemyType _enemyType;
        private List<Character> _characterInRange = new();

        private bool _isDeath;
        private bool _isStopRevival;
        private bool _canDestroyed;
        private bool _isFalled;

        private readonly float _waitTime = 4f;
        private readonly float _waitExplosion = 1f;
        private WaitForSeconds _wait;

        protected override void OnInitialized()
        {
            _enemy = StateMachine.Enemy;
            _enemyType = _enemy.Data.Type;
            _agent = StateMachine.Enemy.NavMeshAgent;
            _fxController = StateMachine.Enemy.FXController;
            
            _collider = GetComponent<Collider>();
        }

        protected override void OnEnter()
        {
            enabled=true;
            _isDeath = false;
            _isFalled = false;

            _agent.isStopped = true;
            _collider.enabled = false;
            _agent.enabled = false;

            StartCoroutine(DieRoutine());
        }

        protected override void OnExit()
        {
            StopAllCoroutines();
            enabled=false;
        }

        private IEnumerator DieRoutine()
        {
            if (_enemyType == EnemyType.Smoker)
            {
                SmokerDie();
                yield return new WaitForSeconds(_waitExplosion);
            }

            
            float time= GetComponent<EnemyAnimController>().GetAnimationClip(4).length;
            _wait = new WaitForSeconds(time);
            yield return  _wait;
            
            
            _isFalled = true;
            
            // if (!_isStopRevival && !_canDestroyed)
            // {
            //     AfterDie();
            // }

            if (_canDestroyed)
            {
                gameObject.SetActive(false);
               // Destroy(gameObject);
            }
            else if (!_isStopRevival)
            {
                AfterDie();
            }
        }

        private void SmokerDie()
        {
            Vector3 explosionPosition = transform.position;
            _characterInRange = AllServices.Container.Single<ISearchService>().GetEntitiesInRange<Character>(explosionPosition, _enemy.Data.ExplosiveAbility.ExplosionRadius);

            foreach (var enemy in _characterInRange)
            {
                if (enemy.IsLife())
                {
                    enemy.ApplyDamage(_enemy.Data.ExplosiveAbility.ExplosiveDamage, ItemType.EnemyExplosion);
                }
            }
        }

        private void AfterDie()
        {
            _collider.enabled = true;
            _agent.enabled = true;
            OnRevival?.Invoke(_enemy);
            _isDeath = false;
            _agent.isStopped = false;
            StateMachine.EnterBehavior<EnemySearchTargetState>();
        }

        public void StopRevival(bool isStopRevival)
        {
            _isStopRevival = isStopRevival;
            _canDestroyed = isStopRevival;
        }

        public void SetDestroyed()
        {
            _canDestroyed = true;
            if (_isFalled)
            {
                Destroy(gameObject);
            }
        }

        public override void Disable()
        {
        }
    }
}