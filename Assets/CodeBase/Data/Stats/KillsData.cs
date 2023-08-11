using System;

namespace Data.Stats
{
    [Serializable]
    public class KillsData
    {
        public int KilledEnemies;
        public int TotalEnemies;

        public float Ratio => KilledEnemies / (float)TotalEnemies;

        public KillsData(int totalEnemies)
        {
            TotalEnemies = totalEnemies;
            Clear();
        }

        public void Increment() =>
            KilledEnemies++;

        public void Clear() =>
            KilledEnemies = (int)ConstantsData.Zero;

        public bool IsTotalKilled() =>
            KilledEnemies == TotalEnemies;
    }
}