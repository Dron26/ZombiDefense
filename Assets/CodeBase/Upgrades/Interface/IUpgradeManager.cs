using System.Collections.Generic;

public interface IUpgradeManager
{
    bool PurchaseUpgrade(int upgradeId);
    bool IsUnlocked(int upgradeId);
    int GetPlayerMoney();
}