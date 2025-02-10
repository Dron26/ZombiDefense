using System;
using System.Collections.Generic;
using Interface;
using Services;
using Services.SaveLoad;

public class UpgradeTree : IUpgradeTree
{
    private Dictionary<int, UpgradeNode> _upgradeNodes = new();
    private ISaveLoadService _saveLoadService;
    private UpgradeHandler _upgradeHandler;
    public UpgradeTree(ISaveLoadService saveLoadService, UpgradeHandler upgradeHandler)
    {
        _saveLoadService=saveLoadService;
        _upgradeHandler=upgradeHandler;
    }

    public void AddUpgrade(Upgrade upgrade, params int[] dependencies)
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

    public bool CanPurchase(int upgradeId, List<Upgrade> unlockedUpgrades, int playerMoney)
    {
        return _upgradeNodes.ContainsKey(upgradeId) &&
               _upgradeNodes[upgradeId].IsAvailable(unlockedUpgrades) &&
               _upgradeNodes[upgradeId].Upgrade.Lock==false;
    }

    public bool PurchaseUpgrade(string upgradeId)
    {
        if (_upgradeHandler.HasPurchasedUpgrade(upgradeId)) return false; 

        _upgradeHandler.AddPurchasedUpgrade(upgradeId);
        AllServices.Container.Single<LoadSaveService>().Save(); 
        AllServices.Container.Single<GameEventBroadcaster>().InvokeOnUpgradePurchased(upgradeId);
        return true;
    }
    
    public Upgrade GetUpgradeById(int upgradeId)
    {
        return _upgradeNodes.ContainsKey(upgradeId) ? _upgradeNodes[upgradeId].Upgrade : null;
    }

    public void SetData(List<UpgradeData> upgradeData)
    {
        foreach (var data in upgradeData)
        {
            _upgradeNodes[data.Id] = new UpgradeNode(new Upgrade(data));
        }

        foreach (var data in upgradeData)
        {
            if (data.UnlockId != 0 && _upgradeNodes.ContainsKey(data.UnlockId))
            {
                _upgradeNodes[data.Id].Dependencies.Add(_upgradeNodes[data.UnlockId]);
            }
        }
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