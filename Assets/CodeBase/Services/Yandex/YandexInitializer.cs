using System.Collections;
using Agava.YandexGames;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.Ads;
using UnityEngine.Events;

namespace Service.Yandex
{
    public class YandexInitializer : MonoCache
    {
        public event UnityAction Completed;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            YandexAdsService yandexAdsService=new YandexAdsService();
            yandexAdsService.Initialize();
            
            YandexGamesSdk.CallbackLogging = true;
        }

        private IEnumerator Start()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            Completed?.Invoke();
            yield break;
#endif

            yield return YandexGamesSdk.Initialize();

            if (YandexGamesSdk.IsInitialized)
            {
                Completed?.Invoke();
            }
            
        }
    }
}
