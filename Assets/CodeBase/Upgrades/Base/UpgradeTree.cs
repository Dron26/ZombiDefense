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
            // Создаем общее улучшение
            Upgrade upgrade = CreateUpgradeFromData(data);
        
            // Если есть зависимости, добавляем их
            if (!string.IsNullOrEmpty(data.UnlockId.ToString()))
            {
                AddUpgrade(upgrade, data.UnlockId);
            }
            else
            {
                AddUpgrade(upgrade);
            }
        }
    }
    
    public Upgrade CreateUpgradeFromData(UpgradeData data)
    {
       
                return new Upgrade( data); // или другой тип
            // Можно добавить другие типы улучшений
            // case UpgradeType.Weapon:
            //     return new WeaponUpgrade(data.Id, data.Name, data.Cost, data.Value);
        
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