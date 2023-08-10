using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.WaveManagment;
using Service.SaveLoadService;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Location
{
    public  class EnemyCharacterInitializer:MonoCache
    {
        [SerializeField] private WaveManager _waveManager;
        
        private  PlayerCharacterInitializer _playerCharacterInitializer;
        private SaveLoadService _saveLoadService;

        public void Initialize(SaveLoadService saveLoadService,SceneInitializer sceneInitializer)
        {
            _playerCharacterInitializer=sceneInitializer.GetPlayerCharacterInitializer();
            
            _saveLoadService = saveLoadService;
            _waveManager.Initialize(saveLoadService,sceneInitializer.GetAudioController());
        }

       

        public WaveManager GetWaveManager() => _waveManager;


        public void StartSpawning()
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