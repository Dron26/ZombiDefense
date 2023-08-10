using Infrastructure.BaseMonoCache.Code.MonoCache;

namespace Service.ADS
{
    public class RewardAds : MonoCache
    {
        public bool TryCanADS()
        {
            print("Позже тут reward рекламу подключить");
            return true;
        }
    }
}