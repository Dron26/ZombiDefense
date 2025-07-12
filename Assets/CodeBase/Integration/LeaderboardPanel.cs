using Infrastructure.BaseMonoCache.Code.MonoCache;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LeaderboardPanel : MonoCache
{
    [SerializeField] private Button _authButton;
    [SerializeField] private GameObject _authObj;
    [SerializeField] private LeaderboardYG _leaderboardYG;
    [SerializeField] private string _tableName = "Leaderboard";

    public void SwitchState(bool isActive)
    {
        if (isActive)
        {
            Debug.LogWarning("ShowLeaderboard");
            ShowLeaderboard();
        }
    }

    private void ShowLeaderboard()
    {
        if (!YG2.player.auth)
        {
            Debug.LogWarning("ShowLeaderboard auth");
            _authObj.GameObject().SetActive(true);
        }
        else
        {
            Debug.LogWarning("ShowLeaderboard noauth");
            _authObj.GameObject().SetActive(false);
        }

        Debug.LogWarning("UpdateLB");
        _leaderboardYG.UpdateLB();
        Debug.LogWarning("_tableName");
        YG2.GetLeaderboard(_tableName);
        Debug.LogWarning("end _tableName");
    }

    protected override void OnEnabled()
    {
        _authButton.onClick.AddListener(() =>
        {
            YG2.OpenAuthDialog();
            _authObj.GameObject().SetActive(false);
        });
    }

    protected override void OnDisabled()
    {
        _authButton.onClick.RemoveListener(() =>
        {
            YG2.OpenAuthDialog();
            _authObj.GameObject().SetActive(false);
        });

    }
}