using System;
using System.Collections;

namespace Service.Ads
{
    public interface IAdsService : IService
    {
        public event Action OnInitializeSuccess;
        public event Action OnClosedVideoAd;
        public event Action<string> OnShowVideoAdError;
        public event Action OnRewardedAd;
        public event Action<bool> OnClosedInterstitialAd;
        public event Action<string> OnShowInterstitialAdError;
        public event Action OnOfflineInterstitialAd;

        bool IsInitialized();
        IEnumerator Initialize();
        void ShowVideoAd();
        void ShowInterstitialAd();
    }
}