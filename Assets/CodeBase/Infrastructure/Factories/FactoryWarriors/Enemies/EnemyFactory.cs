using System;
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

            if (data == null)
                throw new InvalidOperationException(
                    $"Enemy data not found at Resources/{pathData} for enemy type {type}.");

            if (data.prefab == null)
                throw new InvalidOperationException(
                    $"Enemy prefab is not assigned in EnemyData '{data.name}'.");

            GameObject prefab = Instantiate(data.prefab);
            prefab.gameObject.layer = LayerMask.NameToLayer("Character");
            Enemy enemyComponent = prefab.GetComponent<Enemy>();

            if (enemyComponent == null)
            {
                Destroy(prefab);
                throw new InvalidOperationException(
                    $"Prefab '{data.prefab.name}' does not contain a component derived from Enemy.");
            }

            enemyComponent.Initialize(data);
            return enemyComponent;
        }
    }
}