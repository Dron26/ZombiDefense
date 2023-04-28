using Enemies.Pigs;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies.FactoryPig
{
    public class NecromancerPigFactory : PigFactory
    {
        [SerializeField] private int _capacity;
        [SerializeField] private NecromancerPig _prefab;

        private void Awake() => 
            InitEnemy(null, 0);
    }
}