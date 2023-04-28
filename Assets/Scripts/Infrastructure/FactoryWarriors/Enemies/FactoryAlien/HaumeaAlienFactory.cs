using Enemies.Aliens;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies.FactoryAlien
{
    public class HaumeaAlienFactory : AlienFactory
    {
        [SerializeField] private int _capacity;
        [SerializeField] private HaumeaAlien _prefab;

        private void Awake() => 
            InitEnemy(null, 0);
    }
}