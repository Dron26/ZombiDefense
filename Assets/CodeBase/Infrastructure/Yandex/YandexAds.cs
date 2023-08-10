using System;
using Agava.YandexGames;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Constants;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Infrastructure.Yandex
{
    public class YandexAds : MonoCache
    {
        private const int _rewardAmount = 250;
        public Action _adRewarded;
        public bool IsShows { get; private set; }
        private bool _soundStatus = false;
        [SerializeField] private YandexInitializer _yandexInitializer;
        // [SerializeField] private Button _rewardButton;
        public event UnityAction RewardShowed;

        public void Initialize(YandexInitializer yandexInitializer)
        {
            _yandexInitializer=yandexInitializer;
            //    _rewardButton.gameObject.SetActive(false);
        }
    
        protected override void OnEnabled()
        {
            _yandexInitializer.Completed += AppearRewardButton;
            //+= ShowInterstitial;
            // += AppearRewardButton;
            //    _rewardButton.onClick.AddListener(ShowRewardedAd);
        }

        protected override void OnDisabled()
        {
            _yandexInitializer.Completed -= AppearRewardButton;
            // -= ShowInterstitial;
            // -= AppearRewardButton;
            //    _rewardButton.onClick.RemoveListener(ShowRewardedAd);
        }
    
        private void OnLevelWasLoaded(int level)
        {
            string scene =SceneName.Init;
        
            if (SceneManager.GetActiveScene().name!=scene)
            {
                ShowFullScreenAd();
            }
        }

        public void ShowRewardedAd()
        {
            VideoAd.Show(OnOpen, OnRewarded, OnRewardedClosed);
#if YANDEX_GAMES
        VideoAd.Shoe(_adOpened, _adReward, _adClosed, _adErrorOccured);
        _rewardButton.gameObject.SetActive(false);
#endif
#if VK_GAMES
        Agava.VKGames.VideoAd.Show(_adRewarded);
#endif
        }
        public void AppearRewardButton()
        {
            //      _rewardButton.gameObject.SetActive(true);
        }
        public void ShowFullScreenAd()
        {
            print("Ad Shown!");

#if !UNITY_EDITOR
        ShowInterstitial();
#endif
        }
    
        private void ShowInterstitial()
        {
            InterstitialAd.Show();
        }
        public void OnOpen()
        {
            IsShows = true;
            _soundStatus = AudioListener.pause;
            AudioListener.pause = true;
            Time.timeScale = 0;
        }

        public void OnRewarded()
        {
            RewardShowed?.Invoke();
        }

        public void OnRewardedClosed()
        {
            AudioListener.pause = _soundStatus;
            Time.timeScale = 1;
        }

        public void OnFullScreenShowed(bool parameter)
        {
            AudioListener.pause = _soundStatus;
        }

    }
}