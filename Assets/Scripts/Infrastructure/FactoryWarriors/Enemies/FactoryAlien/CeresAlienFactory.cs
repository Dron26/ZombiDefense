using Enemies.Aliens;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies.FactoryAlien
{
    public class CeresAlienFactory : AlienFactory
    {
        [SerializeField] private int _capacity;
        [SerializeField] private CeresAlien _prefab;

        private void Awake() => 
            InitEnemy(_prefab, _capacity);
    }
}