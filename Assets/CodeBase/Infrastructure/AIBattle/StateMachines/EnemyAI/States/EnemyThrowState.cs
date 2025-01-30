using System.Collections;
using Animation;
using Characters.Humanoids.AbstractLevel;
using Data;
using Enemies;
using UnityEngine;
using EnemyData = Enemies.EnemyData;

namespace Infrastructure.AIBattle.StateMachines.EnemyAI.States
{
    public class EnemyThrowState : EnemyState
    {
        private Character _targetCharacter;
        private EnemyData _enemyData;
        private DamageZone _damageZone;
        private bool _isThrowing;
        private EnemyAnimController _enemyAnimController;

        protected override void OnInitialized()
        {
            _enemyData = StateMachine.Enemy.Data;
            _enemyAnimController = StateMachine.Enemy.EnemyAnimController;
        }

        protected override void OnEnter()
        {
            enabled=true;
            _isThrowing = true;
            StartCoroutine(Throw());
        }

        protected override void OnExit()
        {
            _isThrowing = false;
            _enemyAnimController.OnAttack(false);
            enabled=false;
        }

        private IEnumerator Throw()
        {
            // Анимация броска
            _enemyAnimController.OnAttack(true);

            // Ждем завершения анимации броска
            yield return new WaitForSeconds(1f);

            // Создаем зону поражения
            _damageZone = DamageZonePool.Instance.Get();
            _damageZone.Init(_targetCharacter.transform.position, _enemyData.ThrowAbility);

            // Завершаем состояние
            StateMachine.EnterBehavior<EnemySearchTargetState>();
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