using System.Collections.Generic;
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
    }
}