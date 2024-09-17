using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WaveManagment;
using Service.SaveLoad;
using UI.Levels;
using UnityEngine;

namespace Infrastructure.Logic.Inits
{
    public  class EnemyCharacterInitializer:MonoCache
    {
        [SerializeField] private WaveManager _waveManager;
        private  PlayerCharacterInitializer _playerCharacterInitializer;
        private SaveLoadService _saveLoadService;

        public void Initialize(SaveLoadService saveLoadService, SceneInitializer sceneInitializer)
        {
            Debug.Log("Initialize+EnemyCharacterInitializer");
            _playerCharacterInitializer=sceneInitializer.GetPlayerCharacterInitializer();
            _saveLoadService = saveLoadService;
            _waveManager.Initialize(saveLoadService,sceneInitializer);
        }

        public WaveManager GetWaveManager() => _waveManager;


        public void SetWaveData()
        {
            print("StartingSpawning");
            
            _waveManager.StartSpawn(); 
        }
        
        public void StopSpawning()
        {
            print("StopSpawning");
            _waveManager.StopSpawn();
        }
        
    }
}