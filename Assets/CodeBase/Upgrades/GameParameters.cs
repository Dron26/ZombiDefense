using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Logic.WeaponManagment;
using Upgrades;

[Serializable]
public class GameParameters
{
    private List<string> _purchasedUpgrades = new(); 
    private List<string> _unlockedUpgrades = new();
    public bool IsStartParametrs => _isStartParametrs;
    public bool _isStartParametrs;
    public List<string> PurchasedUpgrades => _purchasedUpgrades;
    public List<string> UnlockedUpgrades => _unlockedUpgrades;

    private int _defaultNumberBox;
    public int DefaultNumberBox => _defaultNumberBox;
    
    public void AddPurchasedUpgrade(string key)
    {
        if (!_purchasedUpgrades.Contains(key))
        {
            _purchasedUpgrades.Add(key);
        }
    }

    public bool HasPurchasedUpgrade(string key) => _purchasedUpgrades.Contains(key);
    public bool HasUnlockedUpgrade(string key) => _unlockedUpgrades.Contains(key);

    // Возвращаем список строк вида {upgrade.GroupType}_{upgrade.Type}_{upgrade.Id}
    public HashSet<string> GetUnlockedUpgrades()
    {
        return new HashSet<string>(_unlockedUpgrades);
    }

    public List<string> GetPurchasedUpgradesByType(UpgradeGroupType groupType, UpgradeType type)
    {
        return _purchasedUpgrades
            .Where(upgradeKey => upgradeKey.StartsWith($"{groupType}_{type}"))
            .ToList();
    }
    
    public void AddUnlockedUpgrade(string key)
    {
        if (!_unlockedUpgrades.Contains(key))
        {
            _unlockedUpgrades.Add(key);
        }
    }

    public void ChangeState()
    {
        _isStartParametrs = false;
    }
}