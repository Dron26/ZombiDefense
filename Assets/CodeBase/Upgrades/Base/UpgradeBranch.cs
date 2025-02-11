using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Services
{
    public class UpgradeBranch : MonoCache
    {
        [SerializeField] private UpgradeGroupType _branchTypeGroupType;
        [SerializeField] private List<BranchPoint> _points = new List<BranchPoint>();
        
        private List<Upgrade> _upgrades = new List<Upgrade>();

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
                Upgrade matchingUpgrade = _upgrades.Find(upg => upg.Id == point.GetId && upg.Type == point.GetUpgradeType);
                if (matchingUpgrade != null)
                {
                    point.Initialize(matchingUpgrade);
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
    }
}