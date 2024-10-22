using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.Inits;
using Infrastructure.Logic.WaveManagment;
using UnityEngine;

namespace Infrastructure.Location
{
    public class LocationPrefab:MonoCache
    {
        [SerializeField] private PlayerCharacterInitializer _playerCharacterInitializer;
        [SerializeField] private WaveManager _waveManager;
        [SerializeField] private CameraData _cameraData;
        [SerializeField] private bool _isNight;
        public PlayerCharacterInitializer GetPlayer => _playerCharacterInitializer;
        public WaveManager GetWaveManager => _waveManager;
        public CameraData CameraData => _cameraData;
        public bool IsNight => _isNight;
    }
}