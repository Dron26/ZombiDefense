using System.Collections.Generic;
using Services;

public interface IUpgradeTree:IService
{
    void AddUpgrade(IUpgrade upgrade, params string[] dependencies);
    bool CanPurchase(string upgradeId, List<IUpgrade> unlockedUpgrades, int playerMoney);
    IUpgrade GetUpgradeById(string upgradeId);
}