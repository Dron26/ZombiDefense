using System.Collections.Generic;
using Services;

public interface IUpgradeTree:IService
{
    public void AddUpgrade(Upgrade upgrade, params (string group, int id)[] dependencies);

    public bool CanPurchase(Upgrade upgrade, int playerMoney);
    public bool PurchaseUpgrade(Upgrade upgrade);
    public Upgrade GetUpgradeById(UpgradeGroupType type, int upgradeId);
    public void SetData(List<UpgradeData> upgradeData);
    
    public bool RefundUpgrade(string groupType, int upgradeId, int refundAmount);
    void SetBranch(List<UpgradeBranch> branches);
    public void UpdateBranches();
}