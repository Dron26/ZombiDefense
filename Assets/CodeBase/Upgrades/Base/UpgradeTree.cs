using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.AssetManagement;
using Interface;
using Services;
using Services.SaveLoad;
using UnityEngine;
using Upgrades;

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

    public void AddUpgrade(Upgrade upgrade, params (string group, int id)[] dependencies)
    {
        string nodeKey = $"{upgrade.GroupType}_{upgrade.Type}_{upgrade.Id}";
        var node = new UpgradeNode(upgrade);
        _upgradeNodes[nodeKey] = node;

        foreach (var (group, id) in dependencies)
        {
            string depKey = $"{group}_{id}";
            if (_upgradeNodes.ContainsKey(depKey))
            {
                node.Dependencies.Add(_upgradeNodes[depKey]);
            }
        }
    }

    public bool CanPurchase(Upgrade upgrade, int playerMoney)
    {
        string nodeKey = $"{upgrade.GroupType}_{upgrade.Id}"; 
        
        return _upgradeNodes.ContainsKey(nodeKey) && 
               _upgradeNodes[nodeKey].IsAvailable(_upgradeHandler.GetUnlockedUpgrades()) && 
               !upgrade.Lock && 
               upgrade.Cost <= playerMoney;
    }

    public bool PurchaseUpgrade(Upgrade upgrade)
    {
        string nodeKey = $"{upgrade.GroupType}_{upgrade.Type}_{upgrade.Id}";

        if (_upgradeHandler.HasPurchasedUpgrade(nodeKey)) return false;

        _upgradeHandler.AddPurchasedUpgrade(nodeKey);
        _saveLoadService.Save();
        UpdateBranches();
        return true;
    }

    private void SendAction()
    {
        throw new NotImplementedException();
    }

    public Upgrade GetUpgradeById(UpgradeGroupType type, int upgradeId)
    {
        string nodeKey = $"{type}_{upgradeId}";
        return _upgradeNodes.ContainsKey(nodeKey) ? _upgradeNodes[nodeKey].Upgrade : null;
    }

    public void SetData(List<UpgradeData> upgradeData)
    {
        _upgradeNodes.Clear();
    
        foreach (var data in upgradeData)
        {
            string nodeKey = $"{data.GroupType}_{data.Id}"; 
            _upgradeNodes[nodeKey] = new UpgradeNode(new Upgrade(data));
        
            if (data.Id == 0)
            {
                _upgradeHandler.InitializeBaseUpgrades(_upgradeNodes[nodeKey].Upgrade);
            }
        }

        foreach (var data in upgradeData)
        {
            if (data.UnlockId != 0)
            {
                string nodeKey = $"{data.GroupType}_{data.Id}"; 
                string dependencyKey = $"{data.GroupType}_{data.UnlockId}"; 
                
                if (_upgradeNodes.ContainsKey(dependencyKey))
                {
                    _upgradeNodes[nodeKey].Dependencies.Add(_upgradeNodes[dependencyKey]);
                }
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
    }

    public void UpdateBranches()
    {
        foreach (var branch in _upgradeBranches)
        {
            var branchUpgrades = _upgradeNodes.Values
                .Where(node => node.Upgrade.GroupType == branch.GetUpgradeBranchType)
                .Select(node => node.Upgrade)
                .ToList();

            branch.Initialize(branchUpgrades);
        }
    }

   
}