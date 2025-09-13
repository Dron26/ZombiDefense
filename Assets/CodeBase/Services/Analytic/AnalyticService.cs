using System;
using System.Collections.Generic;
using Data;
using Integration;
using Interface;
using Services.SaveLoad;
using UnityEngine;
using YG;

namespace Services.Analytic
{
    public class AnalyticService : IAnalyticService
    {
        private readonly ISaveLoadService _saveLoadService;
        private readonly IGameTimerTracker _gameTimerTracker;
        private readonly ILocationHandler _locationHandler;

        public AnalyticService()
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _gameTimerTracker = AllServices.Container.Single<IGameTimerTracker>();
            _locationHandler = AllServices.Container.Single<ILocationHandler>();
        }

        private string CurrentLevelName => _locationHandler.GetSelectedLocationId().ToString();

        #region GameEvent

        public void StartGame() => YG2.MetricaSend("game_start");

        public void EndGame() => YG2.MetricaSend("game_end");

        public void WinGame() => YG2.MetricaSend("game_win");

        public void LoseGame() => YG2.MetricaSend("game_lose");

        public void PauseGame() => YG2.MetricaSend("game_pause");

        public void ResumeGame() => YG2.MetricaSend("game_resume");

        public void RestartGame() => YG2.MetricaSend("game_restart");

        public void ExitGame() => YG2.MetricaSend("game_exit");

        #endregion

        #region LevelEvent

        public void StartLevel() 
        {
            _gameTimerTracker.StartLocationTimer();
            YG2.MetricaSend("level_start", "level", CurrentLevelName);
        }

        public void EndLevel()
        {
            var duration = _gameTimerTracker.GetCurrentLocationPlayTime();
            var data = new Dictionary<string, object>
            {
                { "level", CurrentLevelName },
                { "durationMs", (long)duration.TotalMilliseconds }
            };
            YG2.MetricaSend("level_end", data);
        }

        public void WinLevel()
        {
            var duration = _gameTimerTracker.GetCurrentLocationPlayTime();
            var data = new Dictionary<string, object>
            {
                { "level", CurrentLevelName },
                { "result", "win" },
                { "durationMs", (long)duration.TotalMilliseconds }
            };
            YG2.MetricaSend("level_result", data);
        }

        public void LoseLevel()
        {
            var duration = _gameTimerTracker.GetCurrentLocationPlayTime();
            var data = new Dictionary<string, object>
            {
                { "level", CurrentLevelName },
                { "result", "lose" },
                { "durationMs", (long)duration.TotalMilliseconds }
            };
            YG2.MetricaSend("level_result", data);
        }

        public void PauseLevel() => YG2.MetricaSend("level_pause", "level", CurrentLevelName);

        public void ResumeLevel() => YG2.MetricaSend("level_resume", "level", CurrentLevelName);

        public void RestartLevel() => YG2.MetricaSend("level_restart", "level", CurrentLevelName);

        public void ExitLevel() => YG2.MetricaSend("level_exit", "level", CurrentLevelName);

        #endregion

        #region Store

        public void ClickButton(string buttonName) => YG2.MetricaSend("ui_click", "button", buttonName);

        public void BuyCharacter(string characterName) => YG2.MetricaSend("buy_character", "character", characterName);

        public void BuyItem(string itemName) => YG2.MetricaSend("buy_item", "item", itemName);

        public void BuyUpgrade(string upgradeName) => YG2.MetricaSend("buy_upgrade", "upgrade", upgradeName);

        public void BuySkin(string skinName) => YG2.MetricaSend("buy_skin", "skin", skinName);

        #endregion

        public void ApplicationQuit()
        {
            ExitGame();

            GameData data = _saveLoadService.GetGameData();
            AchievementsData a = data.AchievementsData;

            var metrics = new Dictionary<string, object>
            {
                { "totalPlayTimeMs", a.TotalPlayTimeMs },
                { "lastSessionDurationMs", a.LastSessionDurationMs },
                { "lastSessionTimestamp", a.LastSessionTimestamp },
                { "killedEnemies", a.KilledEnemies },
                { "allKilledEnemies", a.AllKilledEnemies },
                { "survivalCount", a.SurvivalCount },
                { "deadCharacterCount", a.CountDeadCharacter },
                { "waveCompletedCount", a.WaveComplatedCount },
                { "money", data.Money.TempMoney },
                { "moneyTotal", data.Money.AllAmountMoney }
            };

            YG2.MetricaSend("session_summary", metrics);
            Debug.Log("[Analytic] Sent session summary to Yandex Metrica.");
        }
    }
}
