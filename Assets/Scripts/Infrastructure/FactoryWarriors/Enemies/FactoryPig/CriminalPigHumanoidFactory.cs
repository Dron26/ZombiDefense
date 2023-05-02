using Enemies.Pigs;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies.FactoryPig
{
    public class CriminalPigHumanoidFactory : PigHumanoidFactory
    {
        [SerializeField] private int _capacity;
        [SerializeField] private CriminalPig _prefab;

    }
}