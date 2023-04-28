using Enemies.Pigs;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies.FactoryPig
{
    public class CriminalPigFactory : PigFactory
    {
        [SerializeField] private int _capacity;
        [SerializeField] private CriminalPig _prefab;

        private void Awake() => 
            InitEnemy(null, 0);
    }
}