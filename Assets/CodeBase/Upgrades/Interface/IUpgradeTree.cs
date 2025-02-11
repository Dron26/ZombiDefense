using System.Collections.Generic;
using Services;

public interface IUpgradeTree:IService
{
    public void AddUpgrade(Upgrade upgrade, params (string group, int id)[] dependencies);
    public bool CanPurchase(UpgradeGroupType type, int upgradeId, HashSet<int> unlockedUpgrades, int playerMoney);
    public bool PurchaseUpgrade(UpgradeGroupType type, int upgradeId);
    public Upgrade GetUpgradeById(UpgradeGroupType type, int upgradeId);
    public void SetData(List<UpgradeData> upgradeData);
    
    public bool RefundUpgrade(string groupType, int upgradeId, int refundAmount);
    void SetBranch(List<UpgradeBranch> branches);
    public void UpdateBranches();
}