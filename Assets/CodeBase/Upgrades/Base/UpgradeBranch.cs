using System;
using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
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

        public void Initialize(List<Upgrade> upgrades)
        {
            _upgrades = upgrades.FindAll(upg => upg.GroupType == _branchTypeGroupType);
            DistributeUpgrades();
            UpdateUI();
        }

        public void DistributeUpgrades()
        {
            foreach (var point in _points)
            {
                Upgrade matchingUpgrade = _upgrades.Find(upg => upg.Id == point.GetId);
                //Debug.Log(matchingUpgrade.GroupType+" "+matchingUpgrade.Type+""+matchingUpgrade.Id);
                
                if (matchingUpgrade != null)
                {
                    point.Initialize(matchingUpgrade);
                    point.Button.onClick.AddListener(()=>OnUpgradeClicked(point.Upgrade));
                }
                else
                {
                    Debug.LogWarning($"Upgrade not found for BranchPoint with ID: {point.GetId} and Type: {point.GetUpgradeType}");
                }
            }
        }

        public void UpdateUI()
        {
            foreach (var point in _points)
            {
                point.RefreshUI();
            }
        }

        public void OnUpgradeClicked(Upgrade upgarde )
        {
            OnUpgradeClick?.Invoke(upgarde);
        }
    }
}