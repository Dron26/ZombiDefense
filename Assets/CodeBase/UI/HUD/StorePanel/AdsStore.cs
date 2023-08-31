using Data;
using Plugins.SoundInstance.Core.Static;
using Service.Ads;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.StorePanel
{
    public class AdsStore:AdsWindow
    {
        [SerializeField] private Button _addMoneyButton;
        [SerializeField] private GameObject _storePanel;
        private int _moneyCount=> ConstantsData.MoneyForReward;
        private  IAdsService _adsService;
        private SaveLoadService _saveLoadService;

        
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
                return;

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

        protected override void AdsServiceInitializedSuccess()
        {
            base.AdsServiceInitializedSuccess();
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
            AdsService.ShowVideoAd();
        }

        private void ShowClosed()
        {
            Debug.Log("OnClosedVideoAd");
            AdsService.OnClosedVideoAd -= ShowClosed;
            SoundInstance.ResumeMusic();
        }

        private void ShowError(string message)
        {
            Debug.Log($"OnErrorFullScreenAd: {message}");
            AdsService.OnShowVideoAdError -= ShowError;
            SoundInstance.ResumeMusic();
        }

        private void AddMoneyAfterAds()
        {
            AddMoney();
            AdsService.OnRewardedAd -= AddMoneyAfterAds;
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
    }
}