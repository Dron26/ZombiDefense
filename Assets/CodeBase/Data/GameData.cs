using System;
using System.Collections.Generic;
using Interface;
using Services;
using Services.SaveLoad;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class GameData
    {
        [SerializeField] private MoneyData _money = new();
        [SerializeField] private AchievementsData _achievementsData = new();
        [SerializeField] private EnemyData _enemyData = new();
        [SerializeField] private CharacterData _characters = new();
        [SerializeField] private Location _location = new();
        [SerializeField] private CameraState _cameraState = new();
        [SerializeField] private AudioData _audioData = new();
        [SerializeField] private TimeStatistics _timeStatistics = new();
        [SerializeField] private ScalingData _scalingData = new();
        [SerializeField] private List<LocationProgressData> _locationProgressData = new();
        [SerializeField] private UpgradeData _upgradeData;
        public MoneyData Money => _money;
        public AchievementsData AchievementsData => _achievementsData;
        public EnemyData EnemyData => _enemyData;
        public CharacterData Characters => _characters;
        public Location Location => _location;
        public CameraState CameraState => _cameraState;
        public AudioData AudioData => _audioData;
        public TimeStatistics TimeStatistics => _timeStatistics;
        public ScalingData Scaling => _scalingData;
        public List<LocationProgressData> LocationProgressData => _locationProgressData;
        public UpgradeData UpgradeData=>_upgradeData;
        
        private const int InitialMoneyAmount = 100;
        [SerializeField] private bool _isFirstStart = true;
        public bool IsFirstStart => _isFirstStart;

        public void ChangeIsFirstStart() => _isFirstStart = false;

        public void AddInitialMoney()
        {
            if (_isFirstStart)
            {
                _money.Money = InitialMoneyAmount;
                ChangeIsFirstStart();
            }
        }
        
        public void UpdateAudioSettings(AudioData audioData) => _audioData = audioData;
        public void OnGameStart() => _timeStatistics.OnGameStart();
        public void OnGameEnd() => _timeStatistics.OnGameEnd();


    }

    [Serializable]
    public class ScalingData
    {
        public float ZombieHealthMultiplier = 1.2f;
        public float RewardMultiplier = 1.1f;
    }
}
