using System;
using Services;

namespace Integration
{
    public interface IGameTimerTracker:IService
    {
        public void StartSession();
        public TimeSpan GetSessionPlayTime();
        public TimeSpan GetTotalPlayTime();
        public DateTime GetLastLoginTime();
        public void StartLocationTimer();
        public TimeSpan GetCurrentLocationPlayTime();
        public string FormatTimeSpan(TimeSpan ts);
        public void ResetPlayTime();
        public void SaveSessionDuration();

    }
}