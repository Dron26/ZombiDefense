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
        private GameData _gameData;
        private GameParameters _gameParameters=new();

        public HashSet<string> GetUnlockedUpgrades() => _gameParameters.GetUnlockedUpgrades();

        public UpgradeHandler(GameParameters gameParameters)
        {
            _gameParameters = gameParameters;
            LoadParametrs();
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

        public GameParameters GetUpgradeData()
        {
            return _gameParameters;
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

        public void InitializeBaseUpgrades(Upgrade upgrade)
        {
            _gameParameters.InitializeBaseUpgrades(upgrade);
        }

        public GameParameters GetGameParametrs()
        {
            return _gameParameters;
        }
        
        
        private void LoadParametrs()
        {
            if (_gameParameters.IsStartParametrs)
            {
                _gameParameters.Initialize(Resources.Load<GameParametersData>(AssetPaths.GameParametersData));
            }
            else
            {
                
            }
            //_startParameters = Resources.Load<StartParameters>(AssetPaths.StartParametrs);
        }
    }
}