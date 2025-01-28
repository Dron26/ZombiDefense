using System;
using Services.SaveLoad;

namespace Data
{
    [Serializable]
    public class GameData
    {
        public MoneyData Money { get; private set; } = new();
        public AchievementsData Achievements { get; private set; } = new();
        public Services.SaveLoad.EnemyData EnemyData { get; private set; } = new();
        public CharacterData Characters { get; private set; } = new();
        public LocationData Locations { get; private set; } = new();
        public CameraState CameraState { get; private set; } = new();

        public AudioData AudioSettings { get; private set; } = new AudioData();
        public TimeStatistics TimeStatistics { get; private set; } = new TimeStatistics();

        public void UpdateAudioSettings(AudioData audioData) => AudioSettings = audioData;
        public void OnGameStart() => TimeStatistics.OnGameStart();
        public void OnGameEnd() => TimeStatistics.OnGameEnd();
    }
}