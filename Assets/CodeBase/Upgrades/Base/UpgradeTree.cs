using System;
using System.Collections.Generic;
using System.Linq;
using Interface;
using Services;
using Services.SaveLoad;

public class UpgradeTree : IUpgradeTree
{
    private Dictionary<int, UpgradeNode> _upgradeNodes = new();
    private ISaveLoadService _saveLoadService;
    private UpgradeHandler _upgradeHandler;
    private List<UpgradeBranch> _upgradeBranches=new List<UpgradeBranch>();
    private IUpgradeLoader _upgradeLoader=new UpgradeLoader();
    public UpgradeTree(ISaveLoadService saveLoadService, UpgradeHandler upgradeHandler)
    {
        _saveLoadService=saveLoadService;
        _upgradeHandler=upgradeHandler;
        SetData(_upgradeLoader.GetData());
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

    public bool CanPurchase(int upgradeId, HashSet<int> unlockedUpgrades, int playerMoney)
    {
        return _upgradeNodes.ContainsKey(upgradeId) &&
               _upgradeNodes[upgradeId].IsAvailable(unlockedUpgrades) &&
               _upgradeNodes[upgradeId].Upgrade.Lock == false;
    }

    public bool PurchaseUpgrade(int upgradeId)
    {
        if (_upgradeHandler.HasPurchasedUpgrade(upgradeId)) return false; 

        _upgradeHandler.AddPurchasedUpgrade(upgradeId);
        AllServices.Container.Single<LoadSaveService>().Save(); 
        AllServices.Container.Single<GameEventBroadcaster>().InvokeOnUpgradePurchased(upgradeId);
        UpdateBranches();
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
    
    public bool RefundUpgrade(int upgradeId, int refundAmount)
    {
        if (_upgradeHandler.RefundUpgrade(upgradeId, refundAmount))
        {
            _saveLoadService.Save(); 
            AllServices.Container.Single<GameEventBroadcaster>().InvokeOnUpgradeRefundedEvent(upgradeId);
            return true;
        }
        return false;
    }

    public void SetBranch(List<UpgradeBranch> branches)
    {
        _upgradeBranches=branches;
    }
    
    public void UpdateBranches()
    {
        foreach (var branch in _upgradeBranches)
        {
            // Фильтруем улучшения по типу группы ветки
            var branchUpgrades = _upgradeNodes.Values
                .Where(node => node.Upgrade.GroupType == branch.GetUpgradeBranchType) // Фильтруем улучшения по ветке
                .Select(node => node.Upgrade)  // Преобразуем в список Upgrade
                .ToList();

            // Инициализируем ветку с отфильтрованными улучшениями
            branch.Initialize(branchUpgrades);
        }
    }
}