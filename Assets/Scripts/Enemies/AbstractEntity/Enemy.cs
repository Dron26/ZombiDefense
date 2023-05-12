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
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Enemies.AbstractEntity
{
    [RequireComponent(typeof(EnemyStateMachine))]
    public abstract class Enemy : MonoCache
    {
        [SerializeField] private AssetReferenceT<EnemyData> enemyDataReference;
        private List<SkinGroup> _skinGroups = new();
        public int MinLevelForHumanoid => enemyData.MinLevelForHumanoid;
        protected EnemyData enemyData;
        public float MaxHealth => enemyData.MaxHealth;
        public float RangeAttack => enemyData.RangeAttack;
        public int Damage => enemyData.Damage;
        public int Level => enemyData.Level; // Добавлено поле Level
        public GameObject GetPrefab() => enemyData.PrefabCharacter;
        public abstract int GetLevel();
        public abstract float GetRangeAttack();
        public abstract int GetDamage();
        public abstract float GetHealth();
        public abstract bool IsLife();
        public abstract int GetPrice();

        private NavMeshAgent _agent;
        public Vector3 StartPosition;

        public abstract void ApplyDamage(float getDamage);

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
                    SetSkin();
                    SetNavMeshSpeed();
                    Load?.Invoke(this);
                }
                else
                {
                    Debug.LogError($"Failed to load enemy data: {handle.OperationException}");
                }
            };
        }


        public abstract void Initialize();

        private void SetSkin()
        {
            foreach (SkinGroup group in transform.GetComponentsInChildren<SkinGroup>())
            {
                group.Initialize();
                group.SetMesh(Random.Range(0, group.GetCountMeshes()));
            }
        }

        private void SetNavMeshSpeed()
        {
            _agent = GetComponent<NavMeshAgent>();
            float minSpeed = 0.6f;
            float maxSpeed = 1.2f;

            if (Level == 4)
            {
                _agent.speed = 0.6f;
            }
            else
            {
                _agent.speed = Random.Range(minSpeed, maxSpeed);
            }
        }
    }
}