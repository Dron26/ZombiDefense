using System.Collections.Generic;
using Services;

public interface IUpgradeManager:IService
{
    bool PurchaseUpgrade(int upgradeId);
    bool IsUnlocked(int upgradeId);
    int GetPlayerMoney();
    public void UpdateUI();

}