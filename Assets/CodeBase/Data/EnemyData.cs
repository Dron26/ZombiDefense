using Enemies;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public EnemyType Type;
         public float MaxHealth;
         public float RangeAttack;
         public int Damage;
         public int Price;
         public int Level;
         public bool IsExplosive;
         public int ExplosiveDamage;
         public float ExplosionRadius;
         
         public RuntimeAnimatorController CharacterController;
         public float NavMeshSpeed;
    }
}