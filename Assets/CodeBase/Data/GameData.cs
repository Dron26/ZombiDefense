
using System;
using Services;
using Services.SaveLoad;

namespace Data
{
    [Serializable]
    public class GameData
    {
        public MoneyData Money { get; private set; } = new();
        public AchievementsData AchievementsData { get; private set; } = new();
        public EnemyData EnemyData { get; private set; } = new();
        public CharacterData Characters { get; private set; } = new();
        public Location Location { get; private set; } = new();
        public CameraState CameraState { get; private set; } = new();

        public AudioData AudioData { get; private set; } = new AudioData();
        public TimeStatistics TimeStatistics { get; private set; } = new TimeStatistics();

        public void UpdateAudioSettings(AudioData audioData) => AudioData = audioData;
        public void OnGameStart() => TimeStatistics.OnGameStart();
        public void OnGameEnd() => TimeStatistics.OnGameEnd();
        
        public bool IsFirstStart { get; private set; } = true;

        public void ChangeIsFirstStart()
        {
            IsFirstStart = false;
        }
    }
}