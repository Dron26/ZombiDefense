using Enemies.Aliens;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies.FactoryAlien
{
    public class ErisAlienHumanoidFactory : AlienHumanoidFactory
    {
        [SerializeField] private int _capacity;
        [SerializeField] private ErisAlien _prefab;

    }
}