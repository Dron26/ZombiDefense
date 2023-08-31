using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class TimeStatistics
    {
        [SerializeField] private float totalPlayTimeInSeconds = 0f;
        [SerializeField] private float playTimeTodayInSeconds = 0f;
        private DateTime lastLoginDate;

        public void OnGameStart()
        {
            lastLoginDate = DateTime.Now;
        }

        public void OnGameEnd()
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan timePlayed = currentTime - lastLoginDate;
            totalPlayTimeInSeconds += (float)timePlayed.TotalSeconds;

            if (lastLoginDate.Date != currentTime.Date)
            {
                playTimeTodayInSeconds = 0f;
            }

            playTimeTodayInSeconds += (float)timePlayed.TotalSeconds;
            lastLoginDate = currentTime;
        }

        public TimeSpan GetTotalPlayTime()
        {
            return TimeSpan.FromSeconds(totalPlayTimeInSeconds);
        }

        public TimeSpan GetPlayTimeToday()
        {
            return TimeSpan.FromSeconds(playTimeTodayInSeconds);
        }
    }
}