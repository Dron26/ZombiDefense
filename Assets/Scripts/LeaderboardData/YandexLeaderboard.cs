using System.Collections;
using System.Collections.Generic;
using Agava.YandexGames;
using Cysharp.Threading.Tasks.Triggers;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YandexLeaderboard : MonoCache
{
    private LeaderboardView _leaderboardView;

    private const string _leaderboardName = "TopPayer";
    private LeaderboardPanel _panel;
    public void Initialize(LeaderboardPanel panel)
    {
        _panel = panel;
        LeaderboardViewHandler viewHandler=_panel.GetComponent<LeaderboardViewHandler>();
        viewHandler.Initialize(this);
        _leaderboardView=viewHandler.GetViwe();
    }

    public void GetResultsFromLeaderboard()
    {
        List<PlayerInfoLeaderboard> playersInfo = new List<PlayerInfoLeaderboard>();
        print("GetResultsFromLeaderboard");
#if UNITY_EDITOR
        for (int i = 0; i < 5; i++)
        {
            playersInfo.Add(new PlayerInfoLeaderboard("name", i));
        }

        _leaderboardView.ConstructLeaderboard(playersInfo);

        return;
#endif

        Leaderboard.GetEntries(_leaderboardName, (result) =>
        {
            Debug.Log($"My rank = {result.userRank}");

            int resultsAmount = result.entries.Length;

            resultsAmount = Mathf.Clamp(resultsAmount, 1, 5);

            for (int i = 0; i < resultsAmount; i++)
            {
                string name = result.entries[i].player.publicName;

                if (string.IsNullOrEmpty(name))
                    name = "Anonymos";

                int score = result.entries[i].score;

                playersInfo.Add(new PlayerInfoLeaderboard(name, score));
            }

            _leaderboardView.ConstructLeaderboard(playersInfo);
        });
    }
}

public class PlayerInfoLeaderboard
{
    public string Name { get; private set; }
    public int Score { get; private set; }

    public PlayerInfoLeaderboard(string name, int score)
    {
        Name = name;
        Score = score;
    }
}
