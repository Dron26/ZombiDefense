using System.Collections.Generic;
using Services;

public interface IUpgradeTree:IService
{
    public void AddUpgrade(Upgrade upgrade, params int[] dependencies);
    public bool CanPurchase(int upgradeId, List<Upgrade> unlockedUpgrades, int playerMoney);

    public bool PurchaseUpgrade(string upgradeId);

    public Upgrade GetUpgradeById(int upgradeId);

    public void SetData(List<UpgradeData> upgradeData);
    
    public bool RefundUpgrade(string upgradeId, int refundAmount);
}