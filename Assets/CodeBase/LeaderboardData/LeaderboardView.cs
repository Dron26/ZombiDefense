using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

public class LeaderboardView : MonoCache
{
    [SerializeField] private Transform _parentObject;
    [SerializeField] private GameObject _leaderboardElementPrefab;

    private YandexLeaderboard _yandexLeaderboard;

    private List<GameObject> _spawnedElements = new List<GameObject>();

    public void SetYandexLeaderboard(YandexLeaderboard yandexLeaderboard)
    {
        _yandexLeaderboard = yandexLeaderboard;
    }

    public void LeaderboardButton()
    {
        _yandexLeaderboard.GetResultsFromLeaderboard();
    }

    public void ConstructLeaderboard(List<PlayerInfoLeaderboard> playersInfo)
    {
        ClearLeaderboard();

        int number = 1;
        
        foreach (PlayerInfoLeaderboard info in playersInfo)
        {
            GameObject leaderboardElementInstance = Instantiate(_leaderboardElementPrefab, _parentObject);

            LeaderboardElement leaderboardElement = leaderboardElementInstance.GetComponent<LeaderboardElement>();
            leaderboardElement.Construct(number,info.Name, info.Score);
            number++;

            _spawnedElements.Add(leaderboardElementInstance);
        }
    }

    private void ClearLeaderboard()
    {
        foreach (var element in _spawnedElements)
            Destroy(element);

        _spawnedElements = new List<GameObject>();
    }
}
