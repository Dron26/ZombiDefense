using System;
using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using UnityEngine;

namespace Services
{
    public class UpgradeBranch : MonoCache
    {
        [SerializeField] private UpgradeGroupType _branchTypeGroupType;
        [SerializeField] private List<BranchPoint> _points = new();
        
        private List<Upgrade> _upgrades = new List<Upgrade>();
        public Action<Upgrade> OnUpgradeClick; 
        public UpgradeGroupType GetUpgradeBranchType => _branchTypeGroupType;
        private IUpgradeHandler _upgradeHandler;
        public void Initialize()
        {
            _upgradeHandler = AllServices.Container.Single<IUpgradeHandler>();
            AddListener();
        }

        public void SetUpgrades(List<Upgrade> upgrades)
        {
            _upgrades = upgrades.FindAll(upg => upg.GroupType == _branchTypeGroupType);
            DistributeUpgrades();
        }
        

        private void DistributeUpgrades()
        {
            for (int i = 0; i < _points.Count; i++)
            {
                Upgrade matchingUpgrade = _upgrades.Find(upg => upg.Id == _points[i].GetId);
                
                if (matchingUpgrade != null)
                {
                    _points[i].Initialize(matchingUpgrade);
                    
                    string nodeKey = $"{matchingUpgrade.GroupType}_{matchingUpgrade.Type}_{matchingUpgrade.Id}";

                    if (_upgradeHandler.HasPurchasedUpgrade(nodeKey))
                    {
                        _points[i].Upgrade.SetPurchased(true);
                    }
                    
                    if(_points[i].GetId==0)
                    {
                        _points[i].IsLock(false); 
                    }
                    else if (_points[i].GetId!=0)
                    {
                        int unlockUpgradeId = _points[i].Upgrade.UnlockId;
                        int unlockUpgradeId2 = _points[unlockUpgradeId].GetId;
                        bool isPurchase = _points[unlockUpgradeId2].Upgrade.IsPurchased;
                        
                        if ( unlockUpgradeId == unlockUpgradeId2&&isPurchase)
                        {
                            _points[i].IsLock(false);
                            _points[i].Upgrade.SetLock(false);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"Upgrade not found for BranchPoint with ID: {_points[i].GetId} and Type: {_points[i].GetUpgradeType}");
                }
            }
        }

        private void OnUpgradeClicked(Upgrade upgarde )
        {
            OnUpgradeClick?.Invoke(upgarde);
        }
        
        private void AddListener()
        {
            foreach (var point in _points)
            {
                point.Button.onClick.AddListener(() => OnUpgradeClicked(point.Upgrade));
            }
        }
    }
}