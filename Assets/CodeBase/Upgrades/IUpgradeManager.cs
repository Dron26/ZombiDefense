using System.Collections.Generic;

public interface IUpgradeManager
{
    bool PurchaseUpgrade(string upgradeId);
    bool IsUnlocked(string upgradeId);
    int GetPlayerMoney();
}