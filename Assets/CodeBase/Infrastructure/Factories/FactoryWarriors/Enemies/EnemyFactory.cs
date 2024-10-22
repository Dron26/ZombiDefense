using Characters.Humanoids.AbstractLevel;
using Data;
using Enemies;
using Enemies.AbstractEntity;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.Audio;
using Service.SaveLoad;
using UnityEngine;

namespace Infrastructure.Factories.FactoryWarriors.Enemies
{
    public class EnemyFactory : MonoCache
    {
        private AudioManager _audioManager;
        
        public Enemy Create(EnemyType type)
        {
            string pathPrefab =AssetPaths.EnemyPrefab + type;
            GameObject prefab = Instantiate(Resources.Load<GameObject>(pathPrefab));
            prefab.gameObject.layer = LayerMask.NameToLayer("Character");
            Enemy enemyComponent = prefab.GetComponent<Enemy>();
            string pathData = AssetPaths.EnemyData + type;
            EnemyData data = Resources.Load<EnemyData>(pathData);
            enemyComponent.Initialize(_audioManager,data);
            return enemyComponent;
        }

        public void Initialize( AudioManager audioManager)
        {
            _audioManager=audioManager;
        }
    }
}