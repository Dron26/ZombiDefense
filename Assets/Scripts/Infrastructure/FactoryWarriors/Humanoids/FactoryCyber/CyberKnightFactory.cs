using Humanoids.Cyber;
using Service.SaveLoadService;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Humanoids.FactoryCyber
{
    public class CyberKnightFactory : CyberFactory
    {
        [SerializeField] private CyberKnight _prefab;
        
        private void Start() => 
            InitHumanoid(_prefab, _saveLoad.ReadAmountHumanoids((int)Level.CyberKnight));
    }
}