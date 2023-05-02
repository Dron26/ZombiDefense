using Enemies.Pigs;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies.FactoryPig
{
    public class BomberPigHumanoidFactory : PigHumanoidFactory
    {
        [SerializeField] private int _capacity;
        [SerializeField] private BomberPig _prefab;

    }
}