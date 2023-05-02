using System.Collections.Generic;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using Infrastructure.FactoryWarriors.Humanoids;
using Infrastructure.Weapon;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Humanoids.AbstractLevel
{
    [RequireComponent(typeof(WeaponController))]
    [RequireComponent(typeof(PlayerCharactersStateMachine))]
    public abstract class Humanoid : MonoCache
    {
        [SerializeField] private AssetReferenceT<HumanoidData> humanoidDataReference;
        [SerializeField] private AssetReferenceT<WeaponData> weaponDataReference;
        
        protected HumanoidData humanoidData;
        protected WeaponData weaponData;
        public delegate void HumanoidDeathHandler(Humanoid humanoid);
        public event HumanoidDeathHandler OnHumanoidDeath;
        public float MaxHealth => humanoidData.MaxHealth;
        public int Level => humanoidData.Level; 
        public  GameObject GetPrefabCharacter()=>humanoidData.PrefabCharacter;
        //public  GameObject GetPrefabWeapon()=>weaponData.WeaponPrefab;
        public  List<GameObject> PrefabCharacterItems=>humanoidData.PrefabCharacterItems;
        public UnityAction<Humanoid> Load;

        public WeaponData GetWeaponData() =>weaponData;

        public abstract int GetLevel();
        public abstract float GetHealth();
        public abstract bool IsLife();
        public abstract int GetPrice();
        
        public abstract int GetDamageDone();

        public void InitPosition(Vector3 newPosition) =>
            transform.position = newPosition;
        
        public abstract void ApplyDamage(int getDamage);

        public void  SetAttacments()
        {
           
        }
        
        public void LoadPrefab()
        {
            if (humanoidDataReference.Asset != null)
            {
                humanoidData = (HumanoidData)humanoidDataReference.Asset;
                Debug.Log($"HumanoidData loaded: {humanoidData}");
                return;
            }
            if (weaponDataReference.Asset != null)
            {
                weaponData = (WeaponData)weaponDataReference.Asset;
                Debug.Log($"HumanoidData loaded: {weaponData}");
                return;
            }
            

            humanoidDataReference.LoadAssetAsync().Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    humanoidData = (HumanoidData)handle.Result;
                    Debug.Log($"HumanoidData loaded: {humanoidData}");
                    Load?.Invoke(this);
                }
                else
                {
                    Debug.LogError($"Failed to load HumanoidData: {handle.OperationException}");
                }
            };
            
            weaponDataReference.LoadAssetAsync().Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    weaponData =(WeaponData)handle.Result;
                    Debug.Log($"CharacterData loaded: {weaponData}");
                    
                }
                else
                {
                    Debug.LogError($"Failed to load enemy data: {handle.OperationException}");
                }
            };
        }

        protected virtual void Die()
        {
            OnHumanoidDeath?.Invoke(this);
            // ...
        }
    }
}