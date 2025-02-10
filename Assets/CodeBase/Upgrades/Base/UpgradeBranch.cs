using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Services
{
    public class UpgradeBranch : MonoCache
    {
        [SerializeField] private UpgradeGroupType _branchGroupType;
        [SerializeField] private List<BranchPoint> _points = new List<BranchPoint>();
        
        private List<Upgrade> _upgrades = new List<Upgrade>();

        public UpgradeGroupType GetUpgradeBranch => _branchGroupType;

        public void Initialize(List<Upgrade> upgrades)
        {
            _upgrades = upgrades.FindAll(upg => upg.GroupType == _branchGroupType);
            DistributeUpgrades();
            UpdateUI();
        }

        private void DistributeUpgrades()
        {
            foreach (var point in _points)
            {
                Upgrade matchingUpgrade = _upgrades.Find(upg => upg.Id == point.GetId && upg.Type == point.GetUpgradeType);
                if (matchingUpgrade != null)
                {
                    point.Initialize(matchingUpgrade);
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