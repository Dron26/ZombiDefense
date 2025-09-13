using System;
using Characters.Humanoids.AbstractLevel;
using Data;
using Enemies.AbstractEntity;
using Interface;
using Unity.VisualScripting;

namespace Services.SaveLoad
{
    public class AchievementsHandler : IAchievementsHandler
    {
        private readonly AchievementsData _achievementsData;
        private readonly IGameEventBroadcaster _gameEvent;

        public AchievementsHandler(AchievementsData achievementsData)
        {
            _achievementsData = achievementsData ?? throw new ArgumentNullException(nameof(achievementsData));
            _gameEvent = AllServices.Container.Single<IGameEventBroadcaster>();
            AddListener();
        }

        public int KilledEnemies => _achievementsData.KilledEnemies;
        public int DailyKilledEnemies => _achievementsData.AllKilledEnemies;
        public int SurvivalCount => _achievementsData.SurvivalCount;
        public int DeadMercenaryCount => _achievementsData.CountDeadCharacter;

        public int WaveComplatedCount => _achievementsData.WaveComplatedCount;
        public bool IsLocationPassed => _achievementsData.IsLocationPassed;

        private void AddKilledEnemy(Enemy enemy)
        {
            _achievementsData.KilledEnemies++;
            _achievementsData.AllKilledEnemies++;
        }

        public void ClearKilledEnemies()
        {
            _achievementsData.KilledEnemies = 0;
            _achievementsData.AllKilledEnemies = 0;
        }

        public void SetSurvivalCount(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Survival count cannot be negative.");
            _achievementsData.SurvivalCount = count;
        }

        public void SetDeadMercenaryCount(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Dead mercenary count cannot be negative.");
            _achievementsData.CountDeadCharacter = count;
        }

        public void SetWaveComplatedCount(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Wave completed count cannot be negative.");
            _achievementsData.WaveComplatedCount = count;
        }

        public long LastDailyRewardTimeBinary { get; }

        public string MoneyLeaderboardTableName => _achievementsData.MoneyLeaderboardTableName;
        public string DeadZombiesLeaderboardTableName => _achievementsData.DeadZombiesLeaderboardTableName;

        public void ResetDailyAchievements()
        {
            _achievementsData.KilledEnemies = 0;
        }

        public void Reset()
        {
            ResetDailyAchievements();
            ClearKilledEnemies();
            _achievementsData.SurvivalCount = 0;
            _achievementsData.CountDeadCharacter = 0;
        }

        public void SetPassedState(bool isPassed)
        {
            _achievementsData.IsLocationPassed = isPassed;
        }

        public void EndTutorial()
        {
            _achievementsData.isTutorialEnd = true;
        }

        bool IAchievementsHandler.IsTutorialEnded() => _achievementsData.isTutorialEnd;


        private void AddListener()
        {
            _gameEvent.OnEnemyDeath += AddKilledEnemy;
            _gameEvent.OnCharacterDie += AddKilledCharacter;
            _gameEvent.OnApplicationQuit += ApplicationQuit;
        }

        private void ApplicationQuit()
        {
            RemoveListener();
            SetPassedState(false);
        }

        private void RemoveListener()
        {
            _gameEvent.OnEnemyDeath -= AddKilledEnemy;
            _gameEvent.OnCharacterDie -= AddKilledCharacter;
            _gameEvent.OnApplicationQuit -= ApplicationQuit;
        }

        public void AddKilledCharacter(Character character)
        {
            _achievementsData.CountDeadCharacter++;
        }

        public bool IsTutorialEnd => _achievementsData.isTutorialEnd;
    }
}