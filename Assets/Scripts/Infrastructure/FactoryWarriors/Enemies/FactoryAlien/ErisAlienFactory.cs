using Enemies.Aliens;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies.FactoryAlien
{
    public class ErisAlienFactory : AlienFactory
    {
        [SerializeField] private int _capacity;
        [SerializeField] private ErisAlien _prefab;

        private void Awake() => 
            InitEnemy(null, 0);
    }
}