using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Plugins.SoundInstance.Core.Static;
using Service;
using Service.Ads;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.StorePanel
{
    public class AdsStore:MonoCache
    {
        [SerializeField] private Button _addMoneyButton;
        [SerializeField] private GameObject _storePanel;
        private int _moneyCount=> ConstantsData.MoneyForReward;
        private  IAdsService _adsService;
        private SaveLoadService _saveLoadService;
        private ISaveLoadService SaveLoadService;

        private void Awake()
        {
            _storePanel.SetActive(false);
        }
        
        protected override void  OnEnabled()
        {
            // _addMoneyButton.enabled = Application.isEditor;
            _addMoneyButton.onClick.AddListener(ShowAds);
            
            if (Application.isEditor)
                return;

            if (_adsService == null)
                _adsService = AllServices.Container.Single<IAdsService>();
            
            if (SaveLoadService == null)
                SaveLoadService = AllServices.Container.Single<ISaveLoadService>();

            _adsService.OnInitializeSuccess += AdsServiceInitializedSuccess;
            _adsService.OnShowVideoAdError += ShowError;
            _adsService.OnClosedVideoAd += ShowClosed;
            _adsService.OnRewardedAd += AddMoneyAfterAds;
            InitializeAdsSDK();
        }

        protected override void  OnDisabled()
        {
            _addMoneyButton.onClick.RemoveListener(ShowAds);

            if (_adsService == null)
                return;

            _adsService.OnInitializeSuccess -= AdsServiceInitializedSuccess;
            _adsService.OnShowVideoAdError -= ShowError;
            _adsService.OnClosedVideoAd -= ShowClosed;
            _adsService.OnRewardedAd -= AddMoneyAfterAds;
        }

        private void AdsServiceInitializedSuccess()
        {
            _adsService.OnInitializeSuccess -= AdsServiceInitializedSuccess;
            _addMoneyButton.enabled = true;
        }

        private void ShowAds()
        {
            if (Application.isEditor)
            {
                AddMoney();
                return;
            }

            SoundInstance.PauseMusic();
            _adsService.ShowVideoAd();
        }

        private void ShowClosed()
        {
            Debug.Log("OnClosedVideoAd");
            _adsService.OnClosedVideoAd -= ShowClosed;
            SoundInstance.ResumeMusic();
        }

        private void ShowError(string message)
        {
            Debug.Log($"OnErrorFullScreenAd: {message}");
            _adsService.OnShowVideoAdError -= ShowError;
            SoundInstance.ResumeMusic();
        }

        private void AddMoneyAfterAds()
        {
            AddMoney();
            _adsService.OnRewardedAd -= AddMoneyAfterAds;
        }

        private void AddMoney()
        {
            Debug.Log("AddMoney");
            
            _saveLoadService.MoneyData.AddMoney(_moneyCount);
            _addMoneyButton.enabled = false;
        }

        public void Initialize(SaveLoadService saveLoadService)
        {
             _saveLoadService=saveLoadService;
        }
        
        private void InitializeAdsSDK()
        {
            Debug.Log("InitializeAdsSDK");
            if (_adsService.IsInitialized())
                AdsServiceInitializedSuccess();
            else
                StartCoroutine(_adsService.Initialize());
        }

    }
}