using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Humanoids
{
    [CreateAssetMenu(fileName = "New Humanoid Data", menuName = "Humanoids/Humanoid Data")]
    public class HumanoidData : ScriptableObject
    {
        [SerializeField] private float maxHealth = 60f;
        [SerializeField] private int level = 1;

        public float MaxHealth => maxHealth;
        public int Level
        {
            get => level;
            set => level = value;
        }
    }
}