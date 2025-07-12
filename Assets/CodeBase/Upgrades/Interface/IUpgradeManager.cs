using System.Collections.Generic;
using Interface;
using Services;
using Services.SaveLoad;
using Upgrades;

public interface IUpgradeManager:IService
{
    public void PurchaseUpgrade(Upgrade upgrade);
    public void SetData(List<UpgradeBranch> branches, UpgradePanel panel);
    public void UpdateBranches();
    public void SetTree();

}