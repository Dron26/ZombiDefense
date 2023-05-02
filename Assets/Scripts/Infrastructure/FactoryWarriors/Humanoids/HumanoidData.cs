using System.Collections.Generic;
using Infrastructure.Weapon;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Humanoids
{
    [CreateAssetMenu(fileName = "New Humanoid Data", menuName = "Humanoids/Humanoid Data")]
    public class HumanoidData : ScriptableObject
    {
        [SerializeField] private float maxHealth = 60f;
        [SerializeField] private GameObject _prefabCharacter;
        [SerializeField] private List<GameObject> _prefabCharacterItems;
        [SerializeField] private int level = 1;

        public float MaxHealth => maxHealth;
        public GameObject PrefabCharacter => _prefabCharacter;
        public List<GameObject> PrefabCharacterItems => _prefabCharacterItems;

        public int Level
        {
            get => level;
            set => level = value;
        }
    }
}