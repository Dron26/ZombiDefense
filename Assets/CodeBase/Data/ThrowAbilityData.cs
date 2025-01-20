using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ThrowAbilityData", menuName = "Abilities/Throw Ability")]
    public class ThrowAbilityData : AbilityData
    {
        public int Damage;
        public float Duration;
        public float TickRate;
        public bool IsInfectious;
        public ParticleSystem ThrowerComponent;
        public ParticleSystem ToxicBoiling;

        private void OnEnable()
        {
            AbilityType = AbilityType.Throw;
        }
    }
}