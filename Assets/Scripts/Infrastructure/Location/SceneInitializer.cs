using System.Collections.Generic;
using System.Numerics;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using Infrastructure.FactoryWarriors.Humanoids;
using Infrastructure.WaveManagment;
using UnityEngine;
using UnityEngine.Events;
using Vector3 = UnityEngine.Vector3;

namespace Infrastructure.Location
{
    public class SceneInitializer:MonoCache
    {
        [SerializeField] private WaveManager _waveManager;
        [SerializeField] private PlayerCharacterInitializer _playerCharacterInitializer;
        public UnityAction SetInfoCompleted;
        private AudioSource _audioSource;
       
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _waveManager.SpawningCompleted += SetInfo;
            _playerCharacterInitializer.AreOverHumanoids+=StopSpawning;
            _playerCharacterInitializer.Initialize(_audioSource);
            _waveManager.Initialize();
        }
        
        public WaveSpawner GetWaveSpawner() => _waveManager.GetWaveSpawner();
        public PlayerCharacterInitializer GetPlayerCharacterInitializer() => _playerCharacterInitializer;

        private void StopSpawning()
        {
            _waveManager.StopSpawn();
        }
        private void SetInfo()
        {
            SetInfoCompleted?.Invoke();
        }
    }
}