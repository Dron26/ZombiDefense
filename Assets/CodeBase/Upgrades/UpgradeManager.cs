using System.Collections.Generic;

public class UpgradeManager : IUpgradeManager
{
    private IUpgradeTree _upgradeTree;
    private List<IUpgrade> _unlockedUpgrades = new();
    private int _playerMoney;

    public UpgradeManager(IUpgradeTree upgradeTree, int startingMoney)
    {
        _upgradeTree = upgradeTree;
        _playerMoney = startingMoney;
    }

    public bool PurchaseUpgrade(string upgradeId)
    {
        if (_upgradeTree.CanPurchase(upgradeId, _unlockedUpgrades, _playerMoney))
        {
            var upgrade = _upgradeTree.GetUpgradeById(upgradeId);
            upgrade.Apply();
            _unlockedUpgrades.Add(upgrade);
            _playerMoney -= upgrade.Cost;
            return true;
        }
        return false;
    }

    public bool IsUnlocked(string upgradeId)
    {
        return _unlockedUpgrades.Exists(u => u.Id == upgradeId);
    }

    public int GetPlayerMoney()
    {
        return _playerMoney;
    }
}