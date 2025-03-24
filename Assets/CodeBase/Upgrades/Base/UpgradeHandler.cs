using System;
using System.Collections.Generic;
using Data;
using Infrastructure.AssetManagement;
using Interface;
using Services.SaveLoad;
using UnityEngine;
using Upgrades;

namespace Services
{
    public class UpgradeHandler : IUpgradeHandler
    {
        private GameParameters _gameParameters;
        private Dictionary<UpgradeGroupType, Action<Upgrade>> _upgradeEvents;

        public HashSet<string> GetUnlockedUpgrades() => _gameParameters.GetUnlockedUpgrades();

        public UpgradeHandler(GameParameters gameParameters)
        {
            _gameParameters = gameParameters;
            _gameParameters.ChangeState();
            _upgradeEvents = new Dictionary<UpgradeGroupType, Action<Upgrade>>();
        }

        public bool HasPurchasedUpgrade(string upgradeId) => _gameParameters.HasPurchasedUpgrade(upgradeId);
        public bool HasUnlockUpgrade(string upgradeId) => _gameParameters.HasUnlockedUpgrade(upgradeId);

        public bool RefundUpgrade(string upgradeId, int refundAmount)
        {
            if (!HasPurchasedUpgrade(upgradeId))
                return false;

            RemovePurchasedUpgrade(upgradeId);
            AllServices.Container.Single<CurrencyHandler>().AddMoney(refundAmount); // Возвращаем деньги игроку
            return true;
        }

        public void AddPurchasedUpgrade(string upgradeId)
        {
            if (!_gameParameters.PurchasedUpgrades.Contains(upgradeId))
            {
                _gameParameters.PurchasedUpgrades.Add(upgradeId);
            }
            
        }

        public void RemovePurchasedUpgrade(string upgradeId)
        {
            if (_gameParameters.PurchasedUpgrades.Contains(upgradeId))
            {
                _gameParameters.PurchasedUpgrades.Remove(upgradeId);
            }
        }

        public void AddUnlockedUpgrade(String key)
        {
            _gameParameters.AddUnlockedUpgrade(key);
        }
        
        public List<string> GetPurchasedUpgradesByType(UpgradeGroupType groupType, UpgradeType type)
        {
            return _gameParameters.GetPurchasedUpgradesByType(groupType, type);
        }
    }
}