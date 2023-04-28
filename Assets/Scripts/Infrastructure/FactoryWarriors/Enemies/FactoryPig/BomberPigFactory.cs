using Enemies.Pigs;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies.FactoryPig
{
    public class BomberPigFactory : PigFactory
    {
        [SerializeField] private int _capacity;
        [SerializeField] private BomberPig _prefab;

        private void Awake() => 
            InitEnemy(null, 0);
    }
}