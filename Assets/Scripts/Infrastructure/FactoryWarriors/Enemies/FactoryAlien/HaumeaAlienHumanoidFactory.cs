using Enemies.Aliens;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies.FactoryAlien
{
    public class HaumeaAlienHumanoidFactory : AlienHumanoidFactory
    {
        [SerializeField] private int _capacity;
        [SerializeField] private HaumeaAlien _prefab;

    }
}