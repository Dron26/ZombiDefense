using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;

namespace Services
{
    public class UpgradeBranch : MonoCache
    {
        [SerializeField] private UpgradeGroupType _branchGroupType;
        private List<Upgrade> _upgrades = new List<Upgrade>();
        private List<BranchPoint> _points = new List<BranchPoint>();
        
        public UpgradeGroupType GetUpgradeBranch => _branchGroupType;

        public void AddUpgrade(Upgrade upgrade)
        {
            if (upgrade != null && upgrade.GroupType == _branchGroupType)
            {
                _upgrades.Add(upgrade);
            }
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
            }
        }

        public void UpdateUI()
        {
            DistributeUpgrades();
            foreach (var point in _points)
            {
                point.RefreshUI(); // Допустим, этот метод обновляет отображение UI
            }
        }
    }
}

public class BranchPoint : MonoCache
{
    [SerializeField] private UpgradeType _upgradetype;
    [SerializeField] private int _id;
    private bool _lock;
    private Sprite _iconUpgrade;
    private Sprite _iconLock;
    private TextMeshProUGUI _info;
    private TextMeshProUGUI _price;
    private string _name;

    public UpgradeType GetUpgradeType => _upgradetype;
    public int GetId => _id;

    public void Initialize(Upgrade data)
    {
        _upgradetype = data.Type;
        _id = data.Id;
        _lock = data.Lock;
        _iconUpgrade = data.IconUpgrade;
        _iconLock = data.IconLock;
        _info.text = data.Info;
        _price.text = data.Price.ToString();
        _name = data.Name;
    }
    
    public void RefreshUI()
    {
       
    }
}