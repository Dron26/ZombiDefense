using Humanoids.Cyber;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Humanoids.FactoryCyber
{
    public class CyberSoldierFactory : CyberFactory
    {
        [SerializeField] private CyberSoldier _prefab;
        
        private void Start() => 
            InitHumanoid(_prefab, _saveLoad.ReadAmountHumanoids((int)Level.CyberSoldier));
    }
}