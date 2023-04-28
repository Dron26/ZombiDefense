using Humanoids.Heroes;
using Service.SaveLoadService;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Humanoids.HeroFactory
{
    public class GunGrandmotherFactory : HeroFactory
    {
        [SerializeField] private GunGrandmother _prefab;
        
        private void Start() => 
            InitHumanoid(_prefab, _saveLoad.ReadAmountHumanoids((int)Level.GunGrandmother));
    }
}