using System.Collections.Generic;
using Data;
using Interface;

namespace Services.SaveLoad
{
    [System.Serializable]
    public class MyYGData
    {
        public MoneyData Money = new();
        public AchievementsData AchievementsData = new();
        public Location Location = new();
        public CameraState CameraState = new();
        public AudioData AudioData = new();
        public TimeStatistics TimeStatistics = new();
        public ScalingData Scaling = new();
        public List<LocationProgressData> LocationProgressData= new();
        public GameParameters GameParameters= new();
        public bool IsFirstStart=true;

    }
}