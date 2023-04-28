using Humanoids.Heroes;
using Service.SaveLoadService;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Humanoids.HeroFactory
{
    public class CyberZombieFactory : HeroFactory
    {
        [SerializeField] private CyberZombie _prefab;
        
        private void Start() => 
            InitHumanoid(_prefab, _saveLoad.ReadAmountHumanoids((int)Level.CyberZombie));
    }
}