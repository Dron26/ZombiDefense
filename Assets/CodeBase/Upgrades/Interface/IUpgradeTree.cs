using System.Collections.Generic;
using Services;

public interface IUpgradeTree:IService
{
    public void AddUpgrade(Upgrade upgrade);
    public bool CanPurchase(Upgrade upgrade, int playerMoney);
    public void PurchaseUpgrade(Upgrade upgrade);
    public Upgrade GetUpgrade(UpgradeData upgradeData);
    public void SetData(List<UpgradeData> upgradeData);
    
    public bool RefundUpgrade(string groupType, int upgradeId, int refundAmount);
    public void SetBranch(List<UpgradeBranch> branches);
    public void UpdateBranches();
    public void UpdateUpgrade(Upgrade upgrade);
    public List<float> GetUpgradeValue(UpgradeGroupType groupType, UpgradeType type);
    public void PurchaseYGBranchUpgrade(Upgrade selectedUpgrade);
}