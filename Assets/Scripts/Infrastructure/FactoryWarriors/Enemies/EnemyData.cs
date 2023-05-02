using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies
{
    [CreateAssetMenu(fileName = "New Enemys Data", menuName = "Enemies/Enemys Data")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private float maxHealth = 60f;
        [SerializeField] private float rangeAttack = 1.2f;
        [SerializeField] private int damage = 15;
        [SerializeField] private GameObject _prefabCharacter;
        [SerializeField] private  List<GameObject>_prefabCharacterItems;
        [SerializeField] private int level = 1;
        [SerializeField] private int _minLevelForHumanoid = 0;

        public float MaxHealth => maxHealth;
        public float RangeAttack => rangeAttack;
        public int Damage => damage;
        
        public GameObject PrefabCharacter => _prefabCharacter;
        public  List<GameObject>PrefabCharacterItems => _prefabCharacterItems;
        public int Level { get => level; set => level = value; }
        public int MinLevelForHumanoid =>_minLevelForHumanoid;
    }
}