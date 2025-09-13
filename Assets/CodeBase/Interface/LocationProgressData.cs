using System;

namespace Interface
{
    [Serializable]
    public class LocationProgressData
    {
        public int Id { get; set; }
        public bool IsTutorial { get; set; }
        public bool IsLocked { get; set; }
        public bool IsCompleted { get; set; }
        
        public int BaseReward { get; set; }
        public int WaveCount { get; set; }
        public int EnemyCount { get; set; }
        public int CurrentWaveLevel { get; set; }
        public bool IsAdditional { get; set; }
        public int UnlockedId { get; set; }

        public string TitleRu { get; set; }
        public string TitleEn { get; set; }
        public string TitleTr { get; set; }
        public string ContextRu { get; set; }
        public string ContextEn { get; set; }
        public string ContextTr { get; set; }
        public string ObjectiveRu { get; set; }
        public string ObjectiveEn { get; set; }
        public string ObjectiveTr { get; set; }
        public string LocationRu { get; set; }
        public string LocationEn { get; set; }
        public string LocationTr { get; set; }
        public string TipRu { get; set; }
        public string TipEn { get; set; }
        public string TipTr { get; set; }

        public LocationProgressData(
            int id,
            bool isTutorial,
            bool isLocked,
            bool isCompleted,
            int baseReward,
            int waveCount,
            int enemyCount,
            int currentWaveLevel,
            bool isAdditional,
            int unlockedId,
            string titleRu,
            string titleEn,
            string titleTr,
            string contextRu,
            string contextEn,
            string contextTr,
            string objectiveRu,
            string objectiveEn,
            string objectiveTr,
            string locationRu,
            string locationEn,
            string locationTr,
            string tipRu,
            string tipEn,
            string tipTr)
        {
            Id = id;
            IsTutorial = isTutorial;
            IsLocked = isLocked;
            IsCompleted = isCompleted;
            BaseReward = baseReward;
            WaveCount = waveCount;
            EnemyCount = enemyCount;
            CurrentWaveLevel = currentWaveLevel;
            IsAdditional = isAdditional;
            UnlockedId = unlockedId;
            TitleRu = titleRu;
            TitleEn = titleEn;
            TitleTr = titleTr;
            ContextRu = contextRu;
            ContextEn = contextEn;
            ContextTr = contextTr;
            ObjectiveRu = objectiveRu;
            ObjectiveEn = objectiveEn;
            ObjectiveTr = objectiveTr;
            LocationRu = locationRu;
            LocationEn = locationEn;
            LocationTr = locationTr;
            TipRu = tipRu;
            TipEn = tipEn;
            TipTr = tipTr;
        }

        public void SetLock(bool isLocked)
        {
            IsLocked = isLocked;
        }

        public void SetCompleted(bool isCompleted)
        {
            IsCompleted = isCompleted;
        }

        public void SetCurrentWaveLevel(int currentWaveLevel)
        {
            CurrentWaveLevel = currentWaveLevel;
        }
    }
}