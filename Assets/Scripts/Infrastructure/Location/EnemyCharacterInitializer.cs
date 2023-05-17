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
        [SerializeField] private Button _ButtonStartSpawning;
        private SaveLoad _saveLoad;

        public void Initialize(SaveLoad saveLoad)
        {
            _saveLoad = saveLoad;
            _waveManager.Initialize(saveLoad);
        }

        public WaveSpawner GetWaveSpawner() => _waveManager.GetWaveSpawner();


        public void StartSpawning()
        {
            _waveManager.StartSpawn(); 
        }
        
        public void StopSpawning()
        {
            _waveManager.StopSpawn();
        }
        
    }
}