using System.Collections;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine.Events;

namespace Services.Yandex
{
    public class YandexInitializer : MonoCache
    {
        public event UnityAction Completed;
        
        private void Awake()
        {
           // DontDestroyOnLoad(gameObject);
           // Completed?.Invoke();
            // YandexAdsService yandexAdsService=new YandexAdsService();
            // yandexAdsService.Initialize();
            //
            // YandexGamesSdk.CallbackLogging = true;
        }

        private IEnumerator Start()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
           // Completed?.Invoke();
            yield break;
#endif
            Completed?.Invoke();
            // yield return YandexGamesSdk.Initialize();
            //
            // if (YandexGamesSdk.IsInitialized)
            // {
            //     Completed?.Invoke();
            // }
        }
    }
}
