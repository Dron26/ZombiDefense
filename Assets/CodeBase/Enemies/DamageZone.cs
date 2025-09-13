using System.Collections;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Data;
using Infrastructure.AIBattle.StateMachines.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Enemies
{
    public class DamageZone : MonoCache
    {
        public int Damage { get; private set; }
        public float Duration { get; private set; }
        public float TickRate { get; private set; }
        public bool IsInfectious { get; private set; }

        private HashSet<Character> _charactersInZone = new HashSet<Character>();
        private EnemyThrowState _throwState;
        private Coroutine _damageCoroutine;

        public void Init(Vector3 position, ThrowAbilityData throwAbility, EnemyThrowState throwState)
        {
            _throwState = throwState;
            transform.position = position;

            Damage = throwAbility.Damage;
            Duration = throwAbility.Duration;
            TickRate = throwAbility.TickRate;
            IsInfectious = throwAbility.IsInfectious;

            _charactersInZone.Clear();

            if (!gameObject.activeInHierarchy)
                gameObject.SetActive(true);

            if (_damageCoroutine != null)
                StopCoroutine(_damageCoroutine);

            _damageCoroutine = StartCoroutine(DamageOverTime());
        }

        private IEnumerator DamageOverTime()
        {
            float elapsed = 0f;

            while (elapsed < Duration)
            {
                foreach (var character in _charactersInZone)
                {
                    if (character.IsLife())
                        character.ApplyDamage(Damage);
                }

                yield return new WaitForSeconds(TickRate);
                elapsed += TickRate;
            }

            _charactersInZone.Clear();

            if (_throwState != null)
                _throwState.ReturnDamageZone(this);
            else
                gameObject.SetActive(false);

            _damageCoroutine = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Character>(out var character))
                _charactersInZone.Add(character);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Character>(out var character))
                _charactersInZone.Remove(character);
        }

        protected override void OnDisable()
        {
            if (_damageCoroutine != null)
            {
                StopCoroutine(_damageCoroutine);
                _damageCoroutine = null;
            }

            _charactersInZone.Clear();
        }
    }
}
