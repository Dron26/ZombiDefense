using Data;
using Interface;
using Services.SaveLoad;

namespace Services
{
    public class UpgradeHandler : IUpgradeHandler
    {
        private GameData _gameData;
        private UpgradeInfo _upgradeInfo;
        public UpgradeHandler(UpgradeInfo upgradeInfo) => _upgradeInfo = upgradeInfo;

        public bool HasPurchasedUpgrade(int upgradeId) => _upgradeInfo.PurchasedUpgrades.Contains(upgradeId);

        public bool RefundUpgrade(int upgradeId, int refundAmount)
        {
            if (!HasPurchasedUpgrade(upgradeId))
                return false;

            RemovePurchasedUpgrade(upgradeId);
            AllServices.Container.Single<CurrencyHandler>().AddMoney(refundAmount); // Возвращаем деньги игроку
            return true;
        }

        public void AddPurchasedUpgrade(int upgradeId)
        {
            if (!_upgradeInfo.PurchasedUpgrades.Contains(upgradeId))
            {
                _upgradeInfo.PurchasedUpgrades.Add(upgradeId);
            }
        }

        public void RemovePurchasedUpgrade(int upgradeId)
        {
            if (!_upgradeInfo.PurchasedUpgrades.Contains(upgradeId))
            {
                _upgradeInfo.PurchasedUpgrades.Remove(upgradeId);
            }
        }
    }
}