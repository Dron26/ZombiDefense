using System.Collections.Generic;
using System.Linq;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services;
using Services.SaveLoad;
using UnityEngine;

namespace Upgrades.Base
{
    public class UpgradeManager : IUpgradeManager
    {
        private IUpgradeTree _upgradeTree;
        private List<Upgrade> _unlockedUpgrades = new();
        private UpgradeInfoPanel _infoPanel;
        private Upgrade _selectedUpgrade;
        private ICurrencyHandler _сurrencyHandler;
        
        
        public UpgradeManager(ICurrencyHandler сurrencyHandler)
        {
            _сurrencyHandler = сurrencyHandler;
        }

        public void SetTree()
        {
            _upgradeTree = AllServices.Container.Single<IUpgradeTree>();

        } 
        public bool PurchaseUpgrade(Upgrade upgrade)
        {
            if (_upgradeTree.CanPurchase(upgrade,_сurrencyHandler.GetCurrentMoney() ))
            {
                _unlockedUpgrades.Add(upgrade);
                _сurrencyHandler.SpendMoney(upgrade.Cost);
                _upgradeTree.PurchaseUpgrade(upgrade);
       //         _upgradeTree.UpdateBranches();

                return true;
            } return false;
        }

        public bool IsUnlocked(int upgradeId)
        {
            return _unlockedUpgrades.Exists(u => u.Id == upgradeId);
        }

        public void UpdateBranches()
        {
            _upgradeTree.UpdateBranches();
        }

        public void SetData(List<UpgradeBranch> branches, UpgradeInfoPanel infoPanel)
        {
            _infoPanel = infoPanel;
            AddListener();
            
            foreach (var branch in branches)
            {
                branch.OnUpgradeClick += OnSelectBranchPoint;
            }
            _upgradeTree.SetBranch(branches);
        }

        private void OnSelectBranchPoint(Upgrade upgrade)
        {
            _selectedUpgrade = upgrade;
            ShowWindow(upgrade);
        }

        private void ShowWindow(Upgrade upgrade)
        {
            _infoPanel.Initialize(upgrade);
            _infoPanel.SwitchState(true);
        }

        private void AddListener()
        {
            _infoPanel.OnApplyClicked += OnApplyClicked;
        }

        private void OnApplyClicked()
        {
            PurchaseUpgrade(_selectedUpgrade);
        }

        public IUpgradeTree GetTree()
        {
            return _upgradeTree;
        }
    }
}