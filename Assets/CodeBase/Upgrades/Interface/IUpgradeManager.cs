using System.Collections.Generic;
using Interface;
using Services;
using Services.SaveLoad;
using Upgrades;

public interface IUpgradeManager:IService
{
    bool PurchaseUpgrade(Upgrade upgrade);
    bool IsUnlocked(int upgradeId);
    public void SetData(List<UpgradeBranch> branches, UpgradePanel panel);
    void UpdateBranches();
    public void SetTree();

}