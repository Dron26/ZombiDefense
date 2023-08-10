using System.Collections;
using Agava.YandexGames;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine.Events;

namespace Infrastructure.Yandex
{
    public class YandexInitializer : MonoCache
    {
        private const string Key = "Key";
        public event UnityAction Completed;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            YandexAds yandexAds=GetComponent<YandexAds>();
            yandexAds.Initialize(this);
            
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

        // public void LoadCloudData()
        // {
        //     Agava.YandexGames.Leaderboard.GetPlayerEntry(LeaderboardConstants.Name, TryCreatePlayerLeaderboardEntity);
        //     Agava.YandexGames.Leaderboard.GetEntries(LeaderboardConstants.Name, _leaderboardPanel.Init, topPlayersCount: _leaderboardPanel.AmountRecords, competingPlayersCount: _leaderboardPanel.AmountRecords);
        // }

        // private void TryCreatePlayerLeaderboardEntity(LeaderboardEntryResponse leaderboardEntryResponse)
        // {
        //     if (leaderboardEntryResponse == null)
        //         Agava.YandexGames.Leaderboard.SetScore(LeaderboardConstants.Name, 0);
        // }
 
    }
}
