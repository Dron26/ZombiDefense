using Services;

namespace Interface
{
    public interface IUpgradeHandler:IService
    {
        void AddPurchasedUpgrade(int upgradeId);
        bool HasPurchasedUpgrade(int upgradeId);
        bool RefundUpgrade(int upgradeId, int refundAmount);
    }
}