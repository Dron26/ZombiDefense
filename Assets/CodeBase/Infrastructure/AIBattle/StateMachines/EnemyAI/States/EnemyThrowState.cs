using System.Collections;
using System.Collections.Generic;
using Animation;
using Characters.Humanoids.AbstractLevel;
using Data;
using Enemies;
using UnityEngine;

namespace Infrastructure.AIBattle.StateMachines.EnemyAI.States
{
    public class EnemyThrowState : EnemyState
    {
        [Header("Damage Zone Pool Settings")]
        [SerializeField] private GameObject _damageZonePrefab;
        [SerializeField] private int _poolSize = 5;

        private Character _targetCharacter;
        private EnemyData _enemyData;
        private EnemyAnimController _enemyAnimController;
        private Queue<DamageZone> _damageZonePool = new Queue<DamageZone>();
        private EnemyFXController _fxController;

        protected override void OnInitialized()
        {
            _enemyData = StateMachine.Enemy.Data;
            _enemyAnimController = StateMachine.Enemy.EnemyAnimController;
            _fxController = StateMachine.Enemy.FXController;

            if (_enemyData.IsThrower)
            {
                InitializeDamageZonePool();
            }
        }

        private void InitializeDamageZonePool()
        {
            if (_damageZonePrefab == null)
            {
                Debug.LogError("Префаб DamageZone не назначен в инспекторе!");
                return;
            }

            for (int i = 0; i < _poolSize; i++)
            {
                var damageZone = CreateDamageZone();
                if (damageZone != null)
                {
                    _damageZonePool.Enqueue(damageZone);
                }
            }
        }

        private DamageZone CreateDamageZone()
        {
            if (_damageZonePrefab == null)
            {
                Debug.LogError("Префаб DamageZone не назначен!");
                return null;
            }

            var damageZoneObj = Instantiate(_damageZonePrefab);
            var damageZone = damageZoneObj.GetComponent<DamageZone>();
            if (damageZone == null)
            {
                Debug.LogError("Префаб DamageZone не содержит компонент DamageZone!");
                Destroy(damageZoneObj);
                return null;
            }

            damageZoneObj.SetActive(false);
            return damageZone;
        }

        protected override void OnEnter()
        {
            if (_targetCharacter == null)
            {
                Debug.LogError("Целевой персонаж не установлен! Переход в поиск цели.");
                StateMachine.EnterBehavior<EnemySearchTargetState>();
                return;
            }

            enabled = true;
            StartCoroutine(Throw());
        }

        protected override void OnExit()
        {
            enabled = false;
        }

        private IEnumerator Throw()
        {
            if (_targetCharacter != null && _targetCharacter.IsLife() && !_targetCharacter.IsMove)
            {
                _enemyAnimController.OnThrowAttack();
               
                yield return new WaitForSeconds(_enemyData.ThrowAbility.TickRate);
            }
            else if(_targetCharacter == null)
                StopCoroutine(_fxController.OnThrowFlesh(_targetCharacter.transform.position));

            StateMachine.EnterBehavior<EnemySearchTargetState>();
            yield return null;
        }

        private void ThrowFlesh()
        {
            StartCoroutine(_fxController.OnThrowFlesh(_targetCharacter.transform.position));
            StartCoroutine(CreateZone());
        }

        private IEnumerator CreateZone()
        {
            yield return new WaitForSeconds(0.8f);
            var damageZone = GetDamageZone();
            damageZone.Init(_targetCharacter.transform.position, _enemyData.ThrowAbility, this);
            transform.LookAt(_targetCharacter.transform.position);
        }

        private DamageZone GetDamageZone()
        {
            if (_damageZonePool.Count == 0)
            {
                Debug.LogWarning("Пул DamageZone пуст! Создается новая зона.");
                var newZone = CreateDamageZone();
                if (newZone == null)
                {
                    Debug.LogError("Не удалось создать новую зону поражения!");
                    return null;
                }
                newZone.gameObject.SetActive(true); // Активируем новую зону
                return newZone;
            }

            var damageZone = _damageZonePool.Dequeue();
            damageZone.gameObject.SetActive(true); // Активируем перед возвратом
            return damageZone;
        }

        public void ReturnDamageZone(DamageZone damageZone)
        {
            if (damageZone == null)
            {
                Debug.LogWarning("Попытка вернуть null DamageZone в пул.");
                return;
            }

            damageZone.gameObject.SetActive(false);
            _damageZonePool.Enqueue(damageZone);
        }

        public override void Disable()
        {
        }

        public void InitCharacter(Character targetCharacter)
        {
            _targetCharacter = targetCharacter;
        }
    }
}