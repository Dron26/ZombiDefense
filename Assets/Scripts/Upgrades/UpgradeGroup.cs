using System.Collections.Generic;
using Humanoids;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrades
{
    public class UpgradeSlot : MonoCache
    {
        [SerializeField] private  Image _first;
        [SerializeField] private  Image _second;
        [SerializeField] private  Image _third;
        [SerializeField] private  Image _fourth;
        [SerializeField] private Image _fifth;
       
        public HumanoidType HumanoidType => _humanoidType;
        private HumanoidType _humanoidType;
        private int _currentLevel;
        
        private List<UpgradeData> _upgradeDatas = new();

        private void Initialize(List<UpgradeData> upgradeDatas, HumanoidType humanoidType, int currentLevel)
        {
            _humanoidType = humanoidType;
            _currentLevel = currentLevel;
            
            for(int i = 0; i < upgradeDatas.Count; i++)
            {
                _upgradeDatas.Add(upgradeDatas[i]);
            }
            
            foreach (UpgradeData upgradeData in upgradeDatas)
            {
                _upgradeDatas.Add(upgradeData);
            }
            
            
        }
    }
}