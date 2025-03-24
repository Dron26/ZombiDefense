using System;
using Services;

namespace Interface
{
    public interface IUpgradeHandler:IService
    {
        void AddPurchasedUpgrade(string upgradeId);
        bool HasPurchasedUpgrade(string upgradeId);
        bool RefundUpgrade(string upgradeId, int refundAmount);
    }
}