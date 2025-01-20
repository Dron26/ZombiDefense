using System.Collections;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Data;
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
        private float _elapsedTime;

        public void Init(Vector3 position, ThrowAbilityData throwAbility)
        {
            transform.position = position;
            Damage = throwAbility.Damage; 
            Duration = throwAbility.Duration;
            TickRate = throwAbility.TickRate;
            IsInfectious = throwAbility.IsInfectious;

            StartCoroutine(DamageOverTime());
        }

        private IEnumerator DamageOverTime()
        {
            while (_elapsedTime < Duration)
            {
                foreach (var character in _charactersInZone)
                {
                    character.ApplyDamage(Damage);
                }

                yield return new WaitForSeconds(TickRate);
                _elapsedTime += TickRate;
            }

            DamageZonePool.Instance.Return(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Character>(out var character))
            {
                _charactersInZone.Add(character);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Character>(out var character))
            {
                _charactersInZone.Remove(character);
            }
        }
    }
}