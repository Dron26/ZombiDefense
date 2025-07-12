using System.Collections.Generic;
using System.Linq;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Integration;
using Interface;
using Services;
using Services.SaveLoad;
using UnityEngine;
using YG;

namespace Upgrades.Base
{
    public class UpgradeManager : IUpgradeManager
    {
        private IUpgradeTree _upgradeTree;
        private List<int> _unlockedUpgradesId = new();
        private UpgradePanel _panel;
        private Upgrade _selectedUpgrade;
        private ICurrencyHandler _сurrencyHandler;
        private PaymentManager _paymentManager;
        private ISaveLoadService _saveLoadService;
        public UpgradeManager(ICurrencyHandler сurrencyHandler, ISaveLoadService saveLoadService)
        {
            _сurrencyHandler = сurrencyHandler;
            _paymentManager = new PaymentManager();
            _saveLoadService = saveLoadService;

        }

        public void SetTree()
        {
            _upgradeTree = AllServices.Container.Single<IUpgradeTree>();

        } 
        public void PurchaseUpgrade(Upgrade upgrade)
        {
            if (_upgradeTree.CanPurchase(upgrade,_сurrencyHandler.GetCurrentMoney() ))
            {
                _сurrencyHandler.SpendMoney(upgrade.Cost);
                _upgradeTree.PurchaseUpgrade(upgrade);
                Debug.Log("PurchaseUpgrade");
                Debug.Log(upgrade.GroupType.ToString()+upgrade.Type.ToString()+upgrade.Id.ToString());
            }
            
            _saveLoadService.Save();
        }

        public void UpdateBranches()
        {
            _upgradeTree.UpdateBranches();
        }

        public void SetData(List<UpgradeBranch> branches, UpgradePanel panel)
        {
            _panel = panel;
            _panel.Initialize();
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
            _panel.SetActive(upgrade);
            _panel.ShowApplyWindow(true);
        }

        private void AddListener()
        {
            _panel.OnApplyClicked += OnApplyClicked;
            _panel.OnApplyClickedYG += OnApplyClickedYG;
        }

        private void OnApplyClicked()
        {
            PurchaseUpgrade(_selectedUpgrade);
        }

        private void OnApplyClickedYG()
        {
            YG2.SetState(_selectedUpgrade.GroupType+_selectedUpgrade.Type.ToString(),_selectedUpgrade.Id);
            Debug.LogWarning("YG2.SetState"+_selectedUpgrade.GroupType+_selectedUpgrade.Type.ToString()+_selectedUpgrade.Id);
            _upgradeTree.PurchaseYGBranchUpgrade(_selectedUpgrade);
            
            _saveLoadService.Save();

        }
        
        public IUpgradeTree GetTree()
        {
            return _upgradeTree;
        }
    }
}