using Enemies.Pigs;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies.FactoryPig
{
    public class NecromancerPigHumanoidFactory : PigHumanoidFactory
    {
        [SerializeField] private int _capacity;
        [SerializeField] private NecromancerPig _prefab;
        
    }
}