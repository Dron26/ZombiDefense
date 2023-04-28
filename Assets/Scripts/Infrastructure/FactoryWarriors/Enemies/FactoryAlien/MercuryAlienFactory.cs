using Enemies.Aliens;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies.FactoryAlien
{
    public class MercuryAlienFactory : AlienFactory
    {
        [SerializeField] private int _capacity;
        [SerializeField] private MercuryAlien _prefab;

        private void Awake() => 
            InitEnemy(null, 0);
    }
}