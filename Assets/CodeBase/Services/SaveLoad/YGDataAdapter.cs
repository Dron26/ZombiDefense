using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Interface;
using UnityEngine;

namespace Services.SaveLoad
{
    public static class YGDataAdapter
    {
        public static MyYGData ToYGData(GameData gameData)
        {
            try
            {
                return new MyYGData
                {
                    Money = gameData.Money,
                    AchievementsData = gameData.AchievementsData,
                    Location = gameData.Location,
                    CameraState = gameData.CameraState,
                    AudioData = gameData.AudioData,
                    TimeStatistics = gameData.TimeStatistics,
                    Scaling = gameData.Scaling,
                    LocationProgressData = gameData.LocationProgressData.ToList(),
                    GameParameters = gameData.GameParameters,
                    IsFirstStart = gameData.IsFirstStart
                };
            }
            catch (Exception ex)
            {
                Debug.LogError($"[YGDataAdapter.ToYGData] Exception: {ex}");
                RuntimeLogger.Instance?.LogError($"[YGDataAdapter.ToYGData] Exception: {ex.Message}");
                return new MyYGData();
            }
        }

        public static void FromYGData(MyYGData ygData, GameData gameData)
        {
            try
            {
                gameData.Money = ygData.Money ?? new MoneyData();
                gameData.AchievementsData = ygData.AchievementsData ?? new AchievementsData();
                gameData.Location = ygData.Location ?? new Location();
                gameData.CameraState = ygData.CameraState ?? new CameraState();
                gameData.AudioData = ygData.AudioData ?? new AudioData();
                gameData.TimeStatistics = ygData.TimeStatistics ?? new TimeStatistics();
                gameData.Scaling = ygData.Scaling ?? new ScalingData();
                gameData.LocationProgressData = ygData.LocationProgressData?.ToList() ?? new List<LocationProgressData>();
                gameData.GameParameters = ygData.GameParameters ?? new GameParameters();
                gameData.IsFirstStart = ygData.IsFirstStart;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[YGDataAdapter.FromYGData] Exception: {ex}");
                RuntimeLogger.Instance?.LogError($"[YGDataAdapter.FromYGData] Exception: {ex.Message}");
            }
        }
    }
}
