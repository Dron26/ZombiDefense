using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ExplosiveAbilityData", menuName = "Abilities/Explosive Ability")]
    public class ExplosiveAbilityData : AbilityData
    {
        public int ExplosiveDamage;
        public float ExplosionRadius;
        public AudioClip ExplosionClip;
    }
}