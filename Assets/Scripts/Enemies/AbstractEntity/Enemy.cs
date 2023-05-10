using System.Collections;
using System.Collections.Generic;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.EnemyAI;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Enemies.AbstractEntity
{
    [RequireComponent(typeof(EnemyStateMachine))]
    public abstract class Enemy : MonoCache
    {
        [SerializeField] private AssetReferenceT<EnemyData> enemyDataReference;
        
        public int MinLevelForHumanoid => enemyData.MinLevelForHumanoid;
        protected EnemyData enemyData;
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

        public Vector3 StartPosition;
        
        public abstract void ApplyDamage(int getDamage);

        public abstract void SetAttacments();
        
        public float GetRadiusSearch() => 25f;
        public UnityAction<Enemy> Load;
        
        protected virtual void Die()
        {
            EnemyStateMachine stateMachine = GetComponent<EnemyStateMachine>();
            stateMachine.EnterBehavior<EnemyDieState>();
        }
        
        public void LoadPrefab()
        {
            if (enemyDataReference.Asset != null)
            {
                enemyData = (Infrastructure.FactoryWarriors.Enemies.EnemyData)enemyDataReference.Asset;
                return;
            }

            enemyDataReference.LoadAssetAsync().Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    enemyData = (Infrastructure.FactoryWarriors.Enemies.EnemyData)handle.Result;
                    Initialize();
                    Load?.Invoke(this);
                }
                else
                {
                    Debug.LogError($"Failed to load enemy data: {handle.OperationException}");
                }
            };
        }

        public abstract void Initialize();
    }
}