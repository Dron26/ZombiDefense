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
        public MoneyData Money = new();
        public AchievementsData AchievementsData = new();
        public Location Location = new();
        public CameraState CameraState = new();
        public AudioData AudioData = new();
        public ScalingData Scaling = new();
        public List<LocationProgressData> LocationProgressData = new();
        public GameParameters GameParameters = new();
        public bool IsFirstStart = true;
        public RemoteConfig RemoteConfig=new RemoteConfig();
        public void ChangeIsFirstStart() => IsFirstStart = false;

        public void UpdateAudioSettings(AudioData audioData) => AudioData = audioData;
    }

    [Serializable]
    public class ScalingData
    {
        public float ZombieHealthMultiplier = 1.2f;
        public float RewardMultiplier = 1.1f;
    }
}