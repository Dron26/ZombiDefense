using System;
using System.Collections.Generic;
using Data;
using Integration;
using Interface;
using Services;
using Services.SaveLoad;
using UI;
using UnityEngine;

public class DailyRewardService
{
    private readonly ISaveLoadService _saveLoadService;
    private readonly GameData _gameData;
    private readonly DailyRewardPanel _rewardPanel;
    private readonly IGameTimerTracker _gameTimeTracker;
    private readonly List<int> _rewardValues;

    public DailyRewardService(DailyRewardPanel rewardPanel )
    {
        _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        _gameData = _saveLoadService.GetGameData();
        _rewardPanel = rewardPanel;
        _gameTimeTracker = AllServices.Container.Single<IGameTimerTracker>();
        _rewardValues = _saveLoadService.GetGameData().RemoteConfig.DailyRewardValues;
        TryShowDailyReward();
    }

    public void TryShowDailyReward()
    {
        DateTime lastLoginDate = _gameTimeTracker.GetLastLoginTime().Date;
        DateTime currentDate = DateTime.UtcNow.Date;

        bool alreadyClaimed = _gameData.AchievementsData.IsRewardClaimedToday;
        bool isNewDay = lastLoginDate != currentDate;
        
        if (isNewDay || !alreadyClaimed)
        {
            
            _rewardPanel.Show(_rewardValues[GetCurrentLoginStreak()], OnClaim, OnDecline);
        }
    }

    public int GetCurrentLoginStreak()
    {
        var data = _saveLoadService.GetGameData();
        return data.AchievementsData.StreakLoginDay;
    }
    
    private void OnClaim(int selectedAmount)
    {
        AllServices.Container.Single<ICurrencyHandler>().AddMoney(selectedAmount);
        _gameData.AchievementsData.IsRewardClaimedToday = true;
        _saveLoadService.Save();
    }

    private void OnDecline()
    {
        _gameData.AchievementsData.IsRewardClaimedToday = false;
    }
}