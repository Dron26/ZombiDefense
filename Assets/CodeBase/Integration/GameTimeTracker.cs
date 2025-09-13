using System;
using Integration;
using Interface;
using Services.SaveLoad;

namespace Services
{
    public class GameTimeTracker : IGameTimerTracker
    {
        private DateTime _sessionStart;
        private TimeSpan _sessionDuration;
        private DateTime _locationEnterTime;

        private ISaveLoadService _saveLoadService;

        public long TotalPlayTimeMs { get; private set; }
        public long LastSessionDurationMs { get; private set; }
        public long LastSessionTimestamp { get; private set; }

        public GameTimeTracker()
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        public void StartSession()
        {
            _sessionStart = DateTime.UtcNow;

            var data = _saveLoadService.GetGameData();
            var lastLogin = DateTime.FromBinary(data.AchievementsData.LastLoginTimeBinary);
            var currentLogin = _sessionStart;

            if (lastLogin.Date != currentLogin.Date)
            {
                if (lastLogin.Date == currentLogin.AddDays(-1).Date)
                {
                    data.AchievementsData.StreakLoginDay++;
                }
                else
                {
                    data.AchievementsData.StreakLoginDay = 1;
                }
            }

            data.AchievementsData.LastLoginTimeBinary = currentLogin.ToBinary();
            _saveLoadService.Save();
        }


        public void SaveSessionDuration()
        {
            _sessionDuration = DateTime.UtcNow - _sessionStart;

            var data = _saveLoadService.GetGameData();

            var previousTotalSeconds = data.AchievementsData.TotalPlayTimeMs / 1000.0;
            var newTotalSeconds = previousTotalSeconds + _sessionDuration.TotalSeconds;

            TotalPlayTimeMs = (long)(newTotalSeconds * 1000);
            LastSessionDurationMs = (long)_sessionDuration.TotalMilliseconds;
            LastSessionTimestamp = new DateTimeOffset(_sessionStart).ToUnixTimeSeconds();

            data.AchievementsData.TotalPlayTimeMs = TotalPlayTimeMs;
            data.AchievementsData.LastSessionDurationMs = LastSessionDurationMs;
            data.AchievementsData.LastSessionTimestamp = LastSessionTimestamp;

            _saveLoadService.Save();
        }

        public TimeSpan GetSessionPlayTime() =>
            DateTime.UtcNow - _sessionStart;

        public TimeSpan GetTotalPlayTime()
        {
            var data = _saveLoadService.GetGameData();
            return TimeSpan.FromMilliseconds(data.AchievementsData.TotalPlayTimeMs);
        }

        public DateTime GetLastLoginTime()
        {
            var data = _saveLoadService.GetGameData();
            return DateTime.FromBinary(data.AchievementsData.LastLoginTimeBinary);
        }

        public void StartLocationTimer()
        {
            _locationEnterTime = DateTime.UtcNow;
        }

        public TimeSpan GetCurrentLocationPlayTime() =>
            DateTime.UtcNow - _locationEnterTime;

        public string FormatTimeSpan(TimeSpan ts) =>
            $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";

        public void ResetPlayTime()
        {
            var data = _saveLoadService.GetGameData();
            data.AchievementsData.TotalPlayTimeMs = 0;
            _saveLoadService.Save();
        }
        
        public bool IsDailyRewardReceivedToday()
        {
            var data = _saveLoadService.GetGameData();
            var lastRewardDate = DateTime.FromBinary(data.AchievementsData.LastDailyRewardTimeBinary);
            return lastRewardDate.Date == DateTime.UtcNow.Date;
        }

        public void MarkDailyRewardReceived()
        {
            var data = _saveLoadService.GetGameData();
            data.AchievementsData.LastDailyRewardTimeBinary = DateTime.UtcNow.ToBinary();
            _saveLoadService.Save();
        }
        
        
    }
}
