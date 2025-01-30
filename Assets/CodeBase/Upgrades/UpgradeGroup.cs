using System;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Data.Upgrades;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Upgrades
{
    public class UpgradeGroup : MonoCache
    {
        [SerializeField] private CharacterType _characterType;
        private UpgradeSlot _upgradeSlot;
        [SerializeField] private List<GameObject> _slots;
        private List<UpgradeSlot> _upgradeSlots=new();
       
        public CharacterType CharacterType => _characterType;
        private int _currentLevel;
        private List<UpgradeData> _upgradeDatas = new();

        public Action<UpgradeData> OnBuyUpgrade;
        
        public void Initialize(List<UpgradeData> upgradeDatas,UpgradeSlot upgradeSlot, int currentLevel)
        {
            _upgradeSlot = upgradeSlot;
            _currentLevel = currentLevel;
            
            bool isLocked = false;
            
            for(int i = 0; i < upgradeDatas.Count; i++)
            {
                if (_currentLevel<i)
                {
                    isLocked = true;
                }
                
                UpgradeSlot newUpgradeSlot = Instantiate(_upgradeSlot,_slots[i].transform);
                newUpgradeSlot.Initialize(i,upgradeDatas[i],isLocked);
                
                newUpgradeSlot.GetUpgrateButton().onClick.AddListener(() => TryBuyUpgrade(i));
                newUpgradeSlot.GetLockeButton().onClick.AddListener(() => OnClickLockUpgrade(i));
                
                _upgradeSlots.Add(newUpgradeSlot);
            }
            
            foreach (UpgradeData upgradeData in upgradeDatas)
            {
                _upgradeDatas.Add(upgradeData);
            }
        }
        
        private void TryBuyUpgrade(int slotIndex)
        {
            OnBuyUpgrade?.Invoke(_upgradeDatas[slotIndex]);
        }
        
        public void SetCurrentLevel(int levelIndex)
        {
            foreach (UpgradeSlot slot in _upgradeSlots)
            {
                if (levelIndex<slot.SlotIndex)
                {
                    slot.IsLocked(true);
                }
            }
        }
        
        private void OnClickLockUpgrade(int slotIndex)
        {
                
        }
    }
}