using System.Collections.Generic;
using Services;

public interface IUpgradeTree:IService
{
    public void AddUpgrade(Upgrade upgrade, params int[] dependencies);
    public bool CanPurchase(int upgradeId, HashSet<int> unlockedUpgrades, int playerMoney);

    public bool PurchaseUpgrade(int upgradeId);

    public Upgrade GetUpgradeById(int upgradeId);

    public void SetData(List<UpgradeData> upgradeData);
    
    public bool RefundUpgrade(int upgradeId, int refundAmount);
    void SetBranch(List<UpgradeBranch> branches);
    public void UpdateBranches();
}