using System;
using Services;

namespace Interface
{
    public interface IUpgradeHandler:IService
    {
        void AddPurchasedUpgrade(string upgradeId);
        bool HasPurchasedUpgrade(string upgradeId);
        bool RefundUpgrade(string upgradeId, int refundAmount);
        GameParameters GetUpgradeData();
        public void Subscribe(UpgradeGroupType groupType, Action<Upgrade> listener);
        public void Unsubscribe(UpgradeGroupType groupType, Action<Upgrade> listener);
        public void TriggerEvent(Upgrade upgrade);
    }
}