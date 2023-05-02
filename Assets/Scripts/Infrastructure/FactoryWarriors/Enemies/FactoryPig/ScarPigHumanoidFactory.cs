using Enemies.Pigs;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies.FactoryPig
{
    public class ScarPigHumanoidFactory : PigHumanoidFactory
    {
        [SerializeField] private int _capacity;
        [SerializeField] private ScarPig _prefab;
        
    }
}