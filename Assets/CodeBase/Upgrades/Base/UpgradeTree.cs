using System.Collections.Generic;
using System.Linq;
using Interface;
using Services;
using Services.SaveLoad;
using UnityEngine;

public class UpgradeTree : IUpgradeTree
{
    private Dictionary<string, UpgradeNode> _upgradeNodes = new();
    private ISaveLoadService _saveLoadService;
    private UpgradeHandler _upgradeHandler;
    private List<UpgradeBranch> _upgradeBranches = new();
    private IUpgradeLoader _upgradeLoader = new UpgradeLoader();
    public UpgradeTree(ISaveLoadService saveLoadService, UpgradeHandler upgradeHandler)
    {
        _saveLoadService = saveLoadService;
        _upgradeHandler = upgradeHandler;
        SetData(_upgradeLoader.GetData());
    }

    public void AddUpgrade(Upgrade upgrade)
    {
        string nodeKey = $"{upgrade.GroupType}_{upgrade.Type}_{upgrade.Id}";
        var node = new UpgradeNode(upgrade);
        _upgradeNodes[nodeKey] = node;
    }

    public void UpdateUpgrade(Upgrade upgrade)
    {
        // Формируем ключ для запроса улучшения
        string nodeKey = $"{upgrade.GroupType}_{upgrade.Type}_{upgrade.Id}";
        
        _upgradeNodes[nodeKey].Upgrade.SetPurchased(true);;
        
        List<UpgradeNode> groupUpgrades = _upgradeNodes.Values
            .Where(node => node.Upgrade.GroupType == upgrade.GroupType)
            .Where(node => node.Upgrade.Type == upgrade.Type)
            .OrderBy(node => node.Upgrade.Id) 
            .ToList();

        for (int i = 0; i < groupUpgrades.Count; i++)
        {
            int unlockUpgradeId = groupUpgrades[i].Upgrade.UnlockId;
            int unlockUpgradeId2 = groupUpgrades[unlockUpgradeId].Upgrade.Id;
            bool isPurchase = groupUpgrades[unlockUpgradeId2].Upgrade.IsPurchased;
            
            if ( unlockUpgradeId == unlockUpgradeId2&&isPurchase)
            {
                groupUpgrades[i].Upgrade.SetLock(false);
                _upgradeHandler.AddUnlockedUpgrade(  $"{groupUpgrades[i].Upgrade.GroupType}_{groupUpgrades[i].Upgrade.Id}");
            }
        }
        
        
    }

    public bool CanPurchase(Upgrade upgrade, int playerMoney)
    {
        string nodeKey = $"{upgrade.GroupType}_{upgrade.Type}_{upgrade.Id}";

        if (_upgradeNodes.ContainsKey(nodeKey)&&!IsUpgradeAdded(nodeKey))
        {
            bool trueOrFalse = _upgradeNodes[nodeKey].IsAvailable(_upgradeHandler.GetUnlockedUpgrades());
            bool trueOrFalse2 = !_upgradeNodes[nodeKey].Upgrade.Lock;
            bool trueOrFalse3 = upgrade.Cost <= playerMoney;

            return trueOrFalse && trueOrFalse2 && trueOrFalse3;
        }
        
        return false;
    }


    public bool IsUpgradeAdded( string nodeKey)
    {
        if (_upgradeHandler.HasPurchasedUpgrade(nodeKey)) return true;

        return false;
    }
    
    public void PurchaseUpgrade(Upgrade upgrade)
    {
        string nodeKey = $"{upgrade.GroupType}_{upgrade.Type}_{upgrade.Id}";

        _upgradeHandler.AddPurchasedUpgrade(nodeKey);

        UpdateUpgrade(upgrade);
        // _upgradeHandler.TriggerEvent(upgrade);
        UpdateBranch(upgrade);
        _saveLoadService.Save();
    }

    public Upgrade GetUpgrade(UpgradeData upgradeData )
    {
        string nodeKey = $"{upgradeData.GroupType}_{upgradeData.Type}_{upgradeData.Id}";
        return _upgradeNodes.ContainsKey(nodeKey) ? _upgradeNodes[nodeKey].Upgrade : null;
    }

    public void SetData(List<UpgradeData> upgradeDatas)
    {
        _upgradeNodes.Clear();

        foreach (var data in upgradeDatas)
        {
            AddUpgrade(new Upgrade(data));

            if (data.Id == 0)
            {
                string nodeKey = $"{data.GroupType}_{data.Type}_{data.Id}";
                var upgrade =_upgradeNodes.ContainsKey(nodeKey) ? _upgradeNodes[nodeKey].Upgrade : null;
                _upgradeHandler.AddUnlockedUpgrade($"{upgrade.GroupType}_{upgrade.Id}");
            }
        }
    }


    public bool RefundUpgrade(string groupType, int upgradeId, int refundAmount)
    {
        string nodeKey = $"{groupType}_{upgradeId}";

        if (_upgradeHandler.RefundUpgrade(nodeKey, refundAmount))
        {
            _saveLoadService.Save();
            AllServices.Container.Single<GameEventBroadcaster>().InvokeOnUpgradeRefundedEvent(upgradeId);
            return true;
        }
        return false;
    }

    public void SetBranch(List<UpgradeBranch> branches)
    {
        _upgradeBranches = branches;
        _upgradeBranches.ForEach(branch => branch.Initialize());
    }

    public void UpdateBranches()
    {
        foreach (var branch in _upgradeBranches)
        {
            var branchUpgrades = _upgradeNodes.Values
                .Where(node => node.Upgrade.GroupType == branch.GetUpgradeBranchType)
                .Select(node => node.Upgrade)
                .ToList();

            branch.SetUpgrades(branchUpgrades);
        }
    }

    public void UpdateBranch(Upgrade upgrade)
    {
        var branch = _upgradeBranches.FirstOrDefault(b => b.GetUpgradeBranchType == upgrade.GroupType);
        if (branch == null) return;

        var branchUpgrades = _upgradeNodes.Values
            .Where(node => node.Upgrade.GroupType == upgrade.GroupType)
            .Select(node => node.Upgrade)
            .ToList();

        branch.SetUpgrades(branchUpgrades);
    }

    public List<float> GetUpgradeValue(UpgradeGroupType groupType, UpgradeType type)
    {
        // Получаем список всех купленных апгрейдов по типу группы и типу апгрейда
        var purchasedUpgrades = _upgradeHandler.GetPurchasedUpgradesByType(groupType, type);
        if (purchasedUpgrades.Count == 0)
        {
            return new List<float> { 0 };
        }

        var lastId = purchasedUpgrades
            .Select(upgradeKey => upgradeKey.Split('_'))
            .Where(parts => parts.Length == 3 && int.TryParse(parts[2], out _))
            .Select(parts => int.Parse(parts[2]))
            .OrderBy(id => id)
            .LastOrDefault();

        if (lastId == 0)
        {
            return new List<float> { 0 };
        }

        var lastPurchasedUpgradeNode = _upgradeNodes.Values
            .FirstOrDefault(node => node.Upgrade.GroupType == groupType 
                                    && node.Upgrade.Type == type 
                                    && node.Upgrade.Id == lastId);

        return lastPurchasedUpgradeNode?.Upgrade.UpgradesValue ?? new List<float> { 0 };
    }
}