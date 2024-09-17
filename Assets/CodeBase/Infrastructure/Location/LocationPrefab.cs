using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.Inits;
using UnityEngine;

namespace Infrastructure.Location
{
    public class LocationPrefab:MonoCache
    {
        [SerializeField] private PlayerCharacterInitializer _playerCharacterInitializer;
        [SerializeField] private EnemyCharacterInitializer _enemyCharacterInitializer;
        [SerializeField] private CameraData _cameraData;
        [SerializeField] private bool _isNight;
        public PlayerCharacterInitializer GetPlayer => _playerCharacterInitializer;
        public EnemyCharacterInitializer GetEnemy => _enemyCharacterInitializer;
        public CameraData CameraData => _cameraData;
        public bool IsNight => _isNight;
    }
}