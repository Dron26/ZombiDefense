using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Service.Ads
{
    public abstract class AdsWindow:MonoCache
    {
        
        protected IAdsService AdsService;

        private void OnEnable()
        {
            if (AdsService == null)
                AdsService = AllServices.Container.Single<IAdsService>();
        }

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