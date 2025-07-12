using Common;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Services;
using Services.Ads;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.StorePanel
{
    public class AdsStore:MonoCache
    {
        // [SerializeField] private Button _addMoneyButton;
        // [SerializeField] private GameObject _storePanel;
        // private int _moneyCount=> Constants.MoneyForReward;
        // private  IAdService _adService;
        // private Wallet _wallet;
        // private ISaveLoadService SaveLoadService;
        //
        // private void Awake()
        // {
        //     _storePanel.SetActive(false);
        // }
        //
        // protected override void  OnEnabled()
        // {
        //     // _addMoneyButton.enabled = Application.isEditor;
        //     _addMoneyButton.onClick.AddListener(ShowAds);
        //     
        //     if (Application.isEditor)
        //         return;
        //
        //     if (_adService == null)
        //         _adService = AllServices.Container.Single<IAdService>();
        //     
        //     if (SaveLoadService == null)
        //         SaveLoadService = AllServices.Container.Single<ISaveLoadService>();
        //
        //     _adService.OnInitializeSuccess += AdsServiceInitializedSuccess;
        //     _adService.OnShowVideoAdError += ShowError;
        //     _adService.OnClosedVideoAd += ShowClosed;
        //     _adService.OnRewardedAd += AddMoneyAfterAds;
        //     InitializeAdsSDK();
        // }
        //
        // protected override void  OnDisabled()
        // {
        //     _addMoneyButton.onClick.RemoveListener(ShowAds);
        //
        //     if (_adService == null)
        //         return;
        //
        //     _adService.OnInitializeSuccess -= AdsServiceInitializedSuccess;
        //     _adService.OnShowVideoAdError -= ShowError;
        //     _adService.OnClosedVideoAd -= ShowClosed;
        //     _adService.OnRewardedAd -= AddMoneyAfterAds;
        // }
        //
        // private void AdsServiceInitializedSuccess()
        // {
        //     _adService.OnInitializeSuccess -= AdsServiceInitializedSuccess;
        //     _addMoneyButton.enabled = true;
        // }
        //
        // private void ShowAds()
        // {
        //     if (Application.isEditor)
        //     {
        //         AddMoney();
        //         return;
        //     }
        //     SoundInstance.PauseMusic();
        //     _adService.ShowVideoAd();
        // }
        //
        // private void ShowClosed()
        // {
        //     Debug.Log("OnClosedVideoAd");
        //     SoundInstance.ResumeMusic();
        // }
        //
        // private void ShowError(string message)
        // {
        //     Debug.Log($"OnErrorFullScreenAd: {message}");
        //     SoundInstance.ResumeMusic();
        // }
        //
        // private void AddMoneyAfterAds()
        // {
        //     AddMoney();
        // }
        //
        // private void AddMoney()
        // {
        //     Debug.Log("AddMoney");
        //     
        //     _wallet.AddMoney(_moneyCount);
        // }
        //
        // public void Initialize(Wallet wallet)
        // {
        //     _wallet=wallet;
        // }
        //
        // private void InitializeAdsSDK()
        // {
        //     Debug.Log("InitializeAdsSDK");
        //     if (_adService.IsInitialized())
        //         AdsServiceInitializedSuccess();
        //     else
        //         StartCoroutine(_adService.Initialize());
        // }

    }
}