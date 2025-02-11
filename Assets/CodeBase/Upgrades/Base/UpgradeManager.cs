using System.Collections.Generic;
using System.Linq;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Services;
using Services.SaveLoad;
using UnityEngine;

namespace Upgrades.Base
{
    public class UpgradeManager : IUpgradeManager
    {
        private IUpgradeTree _upgradeTree;
        private List<Upgrade> _unlockedUpgrades = new();
        private int _playerMoney;
        public UpgradeManager(ISaveLoadService saveLoadService, UpgradeHandler upgradeHandler,int money)
        {
            _upgradeTree = new UpgradeTree(saveLoadService, upgradeHandler);
            _playerMoney = money;
        }

        public bool PurchaseUpgrade(int upgradeId,UpgradeGroupType type)
        {
            var unlockedUpgradesSet = new HashSet<int>(_unlockedUpgrades.Select(u => u.Id));
            
            if (_upgradeTree.CanPurchase(type,upgradeId ,unlockedUpgradesSet, _playerMoney))
            {
                var upgrade = _upgradeTree.GetUpgradeById(type,upgradeId);


                    //применение улучшения
           
                _unlockedUpgrades.Add(upgrade);
                _playerMoney -= upgrade.Cost;
                _upgradeTree.PurchaseUpgrade(type,upgradeId);
                _upgradeTree.UpdateBranches();
                
                return true;
            }
            return false;
        }


        public bool IsUnlocked(int upgradeId)
        {
            return _unlockedUpgrades.Exists(u => u.Id == upgradeId);
        }

        public int GetPlayerMoney()
        {
            return _playerMoney;
        }

        public void UpdateBranches()
        {
            _upgradeTree.UpdateBranches();
        }

        public void SetBranch(List<UpgradeBranch> branches)
        {
            _upgradeTree.SetBranch(branches);
        }
    }
}