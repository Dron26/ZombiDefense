using Data;
using Interface;
using Services.SaveLoad;

namespace Services
{
    public class UpgradeHandler : IUpgradeHandler
    {
        private GameData _gameData;
        private UpgradeData _upgradeData;
        public UpgradeHandler(UpgradeData upgradeData) => _upgradeData = upgradeData;

        public bool HasPurchasedUpgrade(string upgradeId) => _upgradeData.PurchasedUpgrades.Contains(upgradeId);
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
            if (!_upgradeData.PurchasedUpgrades.Contains(upgradeId))
            {
                _upgradeData.PurchasedUpgrades.Add(upgradeId);
            }
        }

        public void RemovePurchasedUpgrade(string upgradeId)
        {
            if (!_upgradeData.PurchasedUpgrades.Contains(upgradeId))
            {
                _upgradeData.PurchasedUpgrades.Remove(upgradeId);
            }
        }
    }
}