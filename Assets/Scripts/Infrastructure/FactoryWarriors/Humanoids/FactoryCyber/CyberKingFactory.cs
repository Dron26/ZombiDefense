using Humanoids.Cyber;
using Service.SaveLoadService;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Humanoids.FactoryCyber
{
    public class CyberKingFactory : CyberFactory
    {
        [SerializeField] private CyberKing _prefab;
        
        private void Start() => 
            InitHumanoid(_prefab, _saveLoad.ReadAmountHumanoids((int)Level.CyberKing));
    }
}