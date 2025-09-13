using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class DailyRewardPanel : MonoBehaviour
{
    [SerializeField] private Button[] rewardButtons;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _panel;
    
    private Action<int> _onClaim;
    private Action _onDecline;

    public void Show(int rewardValue, Action<int> onClaim, Action onDecline)
    {
        _panel.gameObject.SetActive(true);
        _onClaim = onClaim;
        _onDecline = onDecline;

        for (int i = 0; i < rewardButtons.Length; i++)
        {
            int amount = rewardValue;
            _text.text="$"+ amount.ToString();
            rewardButtons[i].onClick.RemoveAllListeners();
            rewardButtons[i].onClick.AddListener(() => Claim(amount));
        }

    }

    private void Claim(int amount)
    {
        _panel.gameObject.SetActive(false);
        _onClaim?.Invoke(amount);
    }

    private void Decline()
    {
        _onDecline?.Invoke();
    }
}
