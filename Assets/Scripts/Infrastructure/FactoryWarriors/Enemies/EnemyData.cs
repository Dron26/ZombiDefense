using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies
{
    [CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemies/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private float maxHealth = 60f;
        [SerializeField] private float rangeAttack = 1.2f;
        [SerializeField] private int damage = 15;
        [SerializeField] private GameObject prefab;
        [SerializeField] private int level = 1;

        public float MaxHealth => maxHealth;
        public float RangeAttack => rangeAttack;
        public int Damage => damage;
        
        public GameObject Prefab => prefab;
        public int Level { get => level; set => level = value; }
    }
}