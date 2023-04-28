using Humanoids.Heroes;
using Service.SaveLoadService;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Humanoids.HeroFactory
{
    public class CrazyTractorFactory : HeroFactory
    {
        [SerializeField] private CrazyTractor _prefab;
        
        private void Start() => 
            InitHumanoid(_prefab, _saveLoad.ReadAmountHumanoids((int)Level.CrazyTractor));
    }
}