using System;

namespace Services.Ads
{
    public interface IAdService: IService
    {
        public void ShowAdInterstitial();
    }
}