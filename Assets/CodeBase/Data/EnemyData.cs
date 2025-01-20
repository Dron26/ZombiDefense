using Enemies;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [Header("Basic Settings")] 
        public GameObject prefab;
        public EnemyType Type;
        public float MaxHealth;
        public float RangeAttack;
        public int Damage;
        public int Price;
        public int Level;


        [Space(10)] [Header("Abilities")] 
        public bool IsThrower;
        public bool IsPolice;
        public bool IsSpecial;
        public bool HasShield;
        public int ShieldHealth;
        
        
        public int ThrowRangeAttack;
        public ThrowAbilityData ThrowAbility;
        public ExplosiveAbilityData ExplosiveAbility;

        [Space(10)]
        [Header("Animation & Navigation")]
        public RuntimeAnimatorController CharacterController;
        public float NavMeshSpeed;
    }
}