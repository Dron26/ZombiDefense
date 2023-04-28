using Enemies.Pigs;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies.FactoryPig
{
    public class ScarPigFactory : PigFactory
    {
        [SerializeField] private int _capacity;
        [SerializeField] private ScarPig _prefab;

        private void Awake() => 
            InitEnemy(null, 0);
    }
}