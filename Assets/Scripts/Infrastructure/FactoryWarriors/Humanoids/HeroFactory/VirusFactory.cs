using Humanoids.Heroes;
using Service.SaveLoadService;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Humanoids.HeroFactory
{
    public class VirusFactory : HeroFactory
    {
        [SerializeField] private Virus _prefab;
        
        private void Start() => 
            InitHumanoid(_prefab, _saveLoad.ReadAmountHumanoids((int)Level.Virus));
    }
}