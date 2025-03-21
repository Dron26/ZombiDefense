using Enemies;
using Enemies.AbstractEntity;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Services.Audio;
using Services.SaveLoad;
using UnityEngine;
using EnemyData = Enemies.EnemyData;

namespace Infrastructure.Factories.FactoryWarriors.Enemies
{
    public class EnemyFactory : MonoCache
    {
        private AudioManager _audioManager;
        
        public Enemy Create(EnemyType type)
        {
            string pathData = AssetPaths.EnemyData + type;
            EnemyData data = Resources.Load<EnemyData>(pathData);
            
            string pathPrefab =AssetPaths.EnemyPrefab + type;
            GameObject prefab = Instantiate(data.prefab);
            prefab.gameObject.layer = LayerMask.NameToLayer("Character");
            Enemy enemyComponent = prefab.GetComponent<Enemy>();
            enemyComponent.Initialize(data);
            return enemyComponent;
        }
    }
}