using System.Collections;
using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoad;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Service.Ads
{
    public abstract class WindowBase:MonoCache
    {
        
        protected IAdsService AdsService;
        protected ILeaderboardService LeaderBoardService;
        protected ISaveLoadService SaveLoadService;

        protected override void OnEnabled()
        {
            if (AdsService == null)
                AdsService = AllServices.Container.Single<IAdsService>();
            if (LeaderBoardService == null)
                LeaderBoardService = AllServices.Container.Single<ILeaderboardService>();
            if (SaveLoadService == null)
                SaveLoadService = AllServices.Container.Single<ISaveLoadService>();
            
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
        
        protected void InitializeLeaderBoard()
        {
            if (LeaderBoardService.IsInitialized())
                RequestLeaderBoard();
            else
                StartCoroutine(CoroutineInitializeLeaderBoard());
        }
        
        protected virtual void RequestLeaderBoard() =>
            LeaderBoardService.OnInitializeSuccess -= RequestLeaderBoard;

        private IEnumerator CoroutineInitializeLeaderBoard()
        {
            yield return LeaderBoardService.Initialize();
        }
        
        protected void ShowSetValueError(string error)
        {
            Debug.Log($"ShowSetValueError {error}");
            LeaderBoardService.OnSetValueError -= ShowSetValueError;
        }

        protected void AddLevelResult()
        {
            Debug.Log($"AddLevelResult {ConstantsData.Leaderboard} {SaveLoadService.LoadData().ReadAmountMoney.ToString()}");
            LeaderBoardService.OnSetValueError += ShowSetValueError;
            SubscribeSetValueSuccess();
            LeaderBoardService.SetValue(ConstantsData.Leaderboard,
                SaveLoadService.LoadData().ReadAmountMoney);
        }
        protected void SuccessSetValue()
        {
            Debug.Log("SuccessSetValue");
            LeaderBoardService.OnSetValueSuccess -= SuccessSetValue;
        }

        protected virtual void SubscribeSetValueSuccess() =>
            LeaderBoardService.OnSetValueSuccess += SuccessSetValue;

    }
}