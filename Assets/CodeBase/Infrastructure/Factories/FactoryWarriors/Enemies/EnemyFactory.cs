using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.Audio;
using Service.SaveLoad;
using UnityEngine;

namespace Infrastructure.Factories.FactoryWarriors.Enemies
{
    public class EnemyFactory : MonoCache
    {
        
        private SaveLoadService _saveLoadService;
        private AudioManager _audioManager;
        
        public Enemy Create(GameObject enemy)
        {
            GameObject newEnemy = Instantiate(enemy);
            Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
            enemyComponent.Initialize(_saveLoadService,_audioManager);
            return enemyComponent;
        }

        public void Initialize(SaveLoadService saveLoadService, AudioManager audioManager)
        {
            _saveLoadService=saveLoadService;
            _audioManager=audioManager;
        }
    }
}