using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;

public class LeaderboardElement : MonoCache
{
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _playerScore;
    [SerializeField] private TMP_Text _playerNumber;
    public void Construct(int number,string name, int score)
    {
        _playerNumber.text=number.ToString();
        _playerName.text = name;
        _playerScore.text = score.ToString();
    }
}
