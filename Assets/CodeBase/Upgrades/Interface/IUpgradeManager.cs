using System.Collections.Generic;
using Services;

public interface IUpgradeManager:IService
{
    bool PurchaseUpgrade(int upgradeId, UpgradeGroupType type);
    bool IsUnlocked(int upgradeId);
    int GetPlayerMoney();
    void SetBranch(List<UpgradeBranch> branches);
    void UpdateBranches();

}