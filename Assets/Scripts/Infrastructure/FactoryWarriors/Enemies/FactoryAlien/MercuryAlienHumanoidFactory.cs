using Enemies.Aliens;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies.FactoryAlien
{
    public class MercuryAlienHumanoidFactory : AlienHumanoidFactory
    {
        [SerializeField] private int _capacity;
        [SerializeField] private MercuryAlien _prefab;

    }
}