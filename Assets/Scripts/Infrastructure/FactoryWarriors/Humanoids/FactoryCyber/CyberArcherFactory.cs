using Humanoids.Cyber;
using Service.SaveLoadService;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Humanoids.FactoryCyber
{
    public class CyberArcherFactory : CyberFactory
    {
        [SerializeField] private CyberArcher _prefab;
        
        private void Start() => 
            InitHumanoid(_prefab, _saveLoad.ReadAmountHumanoids((int)Level.CyberArcher));
    }
}