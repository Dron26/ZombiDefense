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
        private SaveLoad _saveLoad;

        public void Initialize(SaveLoad saveLoad,SceneInitializer sceneInitializer)
        {
            _playerCharacterInitializer=sceneInitializer.GetPlayerCharacterInitializer();
            
            _saveLoad = saveLoad;
            _waveManager.Initialize(saveLoad,sceneInitializer.GetAudioController());
        }

       

        public WaveSpawner GetWaveSpawner() => _waveManager.GetWaveSpawner();


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