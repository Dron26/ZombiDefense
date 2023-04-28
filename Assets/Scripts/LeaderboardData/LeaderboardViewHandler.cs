using UnityEngine;

public class LeaderboardViewHandler : MonoBehaviour
{
    private LeaderboardView _leaderboardView;

    public void Initialize(YandexLeaderboard yandexLeaderboard)
    {
        _leaderboardView = GetComponentInChildren<LeaderboardView>();
        _leaderboardView.SetYandexLeaderboard(yandexLeaderboard);
        _leaderboardView.gameObject.SetActive(false);
    }

    public LeaderboardView GetViwe()
    {
        return _leaderboardView;
    }
}
