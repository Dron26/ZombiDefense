using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Service.Ads
{
    public abstract class AdsWindow:MonoCache
    {
        
        protected IAdsService AdsService;
        
        protected void InitializeAdsSDK()
        {
            Debug.Log("InitializeAdsSDK");
            if (AdsService.IsInitialized())
                AdsServiceInitializedSuccess();
            else
                StartCoroutine(AdsService.Initialize());
        }

        protected virtual void AdsServiceInitializedSuccess() =>
            AdsService.OnInitializeSuccess -= AdsServiceInitializedSuccess;
    }
}