using System.Collections.Generic;
using Interface;
using Services;
using Services.SaveLoad;

public class UpgradeTree : IUpgradeTree
{
    private Dictionary<string, UpgradeNode> _upgradeNodes = new();
    private ISaveLoadService _saveLoadService;
    private IUpgradeHandler _upgradeHandler;
    public UpgradeTree(ISaveLoadService saveLoadService, IUpgradeHandler upgradeHandler)
    {
        _saveLoadService=saveLoadService;
        _upgradeHandler=upgradeHandler;
    }

    public void AddUpgrade(IUpgrade upgrade, params string[] dependencies)
    {
        var node = new UpgradeNode(upgrade);
        _upgradeNodes[upgrade.Id] = node;

        foreach (var dep in dependencies)
        {
            if (_upgradeNodes.ContainsKey(dep))
            {
                node.Dependencies.Add(_upgradeNodes[dep]);
            }
        }
    }

    public bool CanPurchase(string upgradeId, List<IUpgrade> unlockedUpgrades, int playerMoney)
    {
        return _upgradeNodes.ContainsKey(upgradeId) &&
               _upgradeNodes[upgradeId].IsAvailable(unlockedUpgrades) &&
               _upgradeNodes[upgradeId].Upgrade.CanPurchase(playerMoney, unlockedUpgrades);
    }

    public bool PurchaseUpgrade(string upgradeId)
    {
        if (_upgradeHandler.HasPurchasedUpgrade(upgradeId)) return false; 

        _upgradeHandler.AddPurchasedUpgrade(upgradeId);
        AllServices.Container.Single<LoadSaveService>().Save(); 
        AllServices.Container.Single<GameEventBroadcaster>().InvokeOnUpgradePurchased(upgradeId);
        return true;
    }
    
    public IUpgrade GetUpgradeById(string upgradeId)
    {
        return _upgradeNodes.ContainsKey(upgradeId) ? _upgradeNodes[upgradeId].Upgrade : null;
    }

    public void SetData(List<UpgradeData> getData)
    {
        throw new System.NotImplementedException();
    }

    public bool RefundUpgrade(string upgradeId, int refundAmount)
    {
        IUpgradeHandler _upgradeHandler=AllServices.Container.Single<IUpgradeHandler>();
        
        if (_upgradeHandler.RefundUpgrade(upgradeId, refundAmount))
        {
            _saveLoadService.Save(); 
            AllServices.Container.Single<GameEventBroadcaster>().InvokeOnUpgradeRefundedEvent(upgradeId);
            return true;
        }
        return false;
    }
}