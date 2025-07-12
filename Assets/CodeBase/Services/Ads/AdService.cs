using System;
using UnityEngine;
using YG;

namespace Services.Ads
{
    public class AdService : IAdService
    {
        public string rewardVideoID;

    public void Awake()
    {
        YG2.onOpenInterAdv += OnOpenInter;
        YG2.onCloseInterAdvWasShow += OnCloseInter;
        YG2.onErrorInterAdv += OnErrorInter;
        
        YG2.onOpenRewardedAdv += OnOpenReward;
        YG2.onCloseRewardedAdv += OnCloseReward;
        YG2.onErrorRewardedAdv += OnErrorReward; 
        YG2.onRewardAdv += OnReward;
    }


    private void OnDestroy()
    {
        YG2.onOpenInterAdv -= OnOpenInter;
        YG2.onCloseInterAdvWasShow -= OnCloseInter;
        YG2.onErrorInterAdv -= OnErrorInter;
        
        YG2.onOpenRewardedAdv -= OnOpenReward;
        YG2.onCloseRewardedAdv -= OnCloseReward;
        YG2.onErrorRewardedAdv -= OnErrorReward;
        YG2.onRewardAdv -= OnReward;
    }

   
    public void ShowAdInterstitial()
    {
        Debug.Log("YG2: Calling InterstitialAdvShow()");
        YG2.InterstitialAdvShow();
    }

    private void OnOpenInter()
    {
        Debug.Log("YG2: Interstitial ad opened");
    }

    private void OnCloseInter(bool wasShown)
    {
        Debug.Log($">> YG2: Interstitial closed; wasShown = {wasShown}");
    }

    private void OnErrorInter()
    {
        Debug.LogError("YG2: Error opening interstitial ad");
    }

    public void ShowRewardedAd()
    {
        Debug.Log("YG2: Calling RewardedAdvShow()");
        YG2.RewardedAdvShow(rewardVideoID, () => {
            Debug.Log($"YG2: Rewarded direct callback for id = {rewardVideoID}");
            GiveReward();
        });
    }
    
    private void OnReward(string id)
    {
        if (id == rewardVideoID)
        {
            Debug.Log($"YG2: onRewardAdv fired for id = {id}");
            GiveReward();
        }
    }

    private void GiveReward()
    {}

    private void OnOpenReward()
    {
        Debug.Log("YG2: Rewarded ad opened");
    }

    private void OnCloseReward()
    {
        Debug.Log("YG2: Rewarded ad closed");
    }

    private void OnErrorReward()
    {
        Debug.LogError("YG2: Error opening rewarded ad");
    }
    }
}