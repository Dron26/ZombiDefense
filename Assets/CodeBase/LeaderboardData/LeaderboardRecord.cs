using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;

public class LeaderboardRecord : MonoCache
{
    [SerializeField] private TMP_Text _place;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _score;

    // public void UpdateData(LeaderboardEntryResponse leaderboardEntryResponse, int place = 0)
    // {
    //     if (place == 0)
    //         place = leaderboardEntryResponse.rank;
    //
    //     SetData(leaderboardEntryResponse, place);
    //
    // }
    //
    // private void SetData(LeaderboardEntryResponse leaderboardEntryResponse, int place)
    // {
    //     _place.text = place.ToString();
    //     _score.text = NumberCuter.Execute(leaderboardEntryResponse.score);
    //
    //     if (leaderboardEntryResponse.player.publicName == string.Empty || leaderboardEntryResponse.player.publicName == null)
    //         return;
    //
    //     _name.text = leaderboardEntryResponse.player.publicName;
    // }
}