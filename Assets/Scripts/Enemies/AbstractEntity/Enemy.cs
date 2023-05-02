﻿using Infrastructure.AIBattle;
using Infrastructure.AIBattle.EnemyAI;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Enemies.AbstractEntity
{
    [RequireComponent(typeof(EnemyStateMachine))]
    public abstract class Enemy : MonoCache
    {
        [SerializeField] private AssetReferenceT<EnemyData> enemyDataReference;
        
        public int MinLevelForHumanoid => enemyData.MinLevelForHumanoid;
        protected EnemyData enemyData;
        public delegate void EnemyDeathHandler(Enemy enemy);
        public event EnemyDeathHandler OnEnemyDeath;
        public float MaxHealth => enemyData.MaxHealth;
        public float RangeAttack => enemyData.RangeAttack;
        public int Damage => enemyData.Damage;
        public int Level => enemyData.Level; // Добавлено поле Level
        public  GameObject GetPrefab()=>enemyData.PrefabCharacter;
        public abstract int GetLevel();
        public abstract float GetRangeAttack();
        public abstract int GetDamage();
        public abstract float GetHealth();
        public abstract bool IsLife();
        public abstract int GetPrice();
        
        public abstract void ApplyDamage(int getDamage);

        public abstract void SetAttacments();
        
        public float GetRadiusSearch() => 25f;
        
        protected virtual void Die()
        {
            OnEnemyDeath?.Invoke(this);
            // ...
        }
        
        public void LoadPrefab()
        {
            if (enemyDataReference.Asset != null)
            {
                enemyData = (Infrastructure.FactoryWarriors.Enemies.EnemyData)enemyDataReference.Asset;
                Debug.Log($"EnemyData loaded: {enemyData}");
                return;
            }

            enemyDataReference.LoadAssetAsync().Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    enemyData = (Infrastructure.FactoryWarriors.Enemies.EnemyData)handle.Result;
                    Debug.Log($"EnemyData loaded: {enemyData}");
                }
                else
                {
                    Debug.LogError($"Failed to load enemy data: {handle.OperationException}");
                }
            };
        }
    }
}