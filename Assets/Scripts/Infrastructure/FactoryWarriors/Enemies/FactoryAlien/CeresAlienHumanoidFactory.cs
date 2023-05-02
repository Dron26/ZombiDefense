using Enemies.Aliens;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies.FactoryAlien
{
    public class CeresAlienHumanoidFactory : AlienHumanoidFactory
    {
        [SerializeField] private int _capacity;
        [SerializeField] private CeresAlien _prefab;

    }
}