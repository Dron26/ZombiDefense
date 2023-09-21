using System.Collections.Generic;

namespace Data
{
    public class LocationData
    {
        public int Id;
        public int MaxEnemyOnLevel;
        public string Path;
        public bool IsTutorial;
        public bool IsLocked;
        public bool IsCompleted;
        public List<WaveData> WaveDatas;
    }
}