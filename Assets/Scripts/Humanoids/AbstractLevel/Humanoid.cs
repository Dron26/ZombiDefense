using System.Collections.Generic;
using System.Threading.Tasks;
using Audio;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Humanoids;
using Infrastructure.WeaponManagment;
using Observer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Humanoids.AbstractLevel
{
    [RequireComponent(typeof(WeaponController))]
    [RequireComponent(typeof(PlayerCharactersStateMachine))]
    public abstract class Humanoid : MonoCache, IObservableHumanoid
    {
        [SerializeField] private AssetReferenceT<HumanoidData> humanoidDataReference;
        [SerializeField] private AssetReferenceT<WeaponData> weaponDataReference;
       
        public Vector3 StartPosition;
        private int _countLoaded = 0;
        private int _maxCountLoaded = 2;
        private AudioController _audioController;
        private List<IObserverByHumanoid> observers = new List<IObserverByHumanoid>();
        
        public UnityAction<Humanoid> OnDataLoad;
        public UnityAction<Humanoid> OnHumanoidSelected;
        
        public Sprite sprite;

        protected HumanoidData humanoidData;
        protected WeaponData weaponData;
        public bool IsSelected => _isSelected;
        public float MaxHealth => humanoidData.MaxHealth;
        public int Level => humanoidData.Level;
        public abstract int GetLevel();
        public abstract float GetHealth();
        public abstract bool IsLife();
        public abstract int GetPrice();
        public abstract int GetDamageDone();

        private bool _isSelected;

        public WeaponData GetWeaponData() => weaponData;
        public abstract void ApplyDamage(int getDamage);


        public Task LoadPrefab()
        {
            var tcs = new TaskCompletionSource<bool>();

            if (humanoidDataReference.Asset != null)
            {
                humanoidData = (HumanoidData)humanoidDataReference.Asset;
                Debug.Log($"HumanoidData loaded: {humanoidData}");
            }

            if (weaponDataReference.Asset != null)
            {
                weaponData = (WeaponData)weaponDataReference.Asset;
                Debug.Log($"HumanoidData loaded: {weaponData}");
            }

            humanoidDataReference.LoadAssetAsync().Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    humanoidData = (HumanoidData)handle.Result;
                    Debug.Log($"HumanoidData loaded: {humanoidData}");
                    tcs.TrySetResult(true);
                    NotifyObservers(this);
                }
                else
                {
                    Debug.LogError($"Failed to load HumanoidData: {handle.OperationException}");
                    tcs.TrySetException(handle.OperationException);
                }
            };

            weaponDataReference.LoadAssetAsync().Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    weaponData = (WeaponData)handle.Result;
                    Debug.Log($"weaponData loaded: {weaponData}");
                    tcs.TrySetResult(true);
                    NotifyObservers(this);
                }
                else
                {
                    Debug.LogError($"Failed to load enemy data: {handle.OperationException}");
                    tcs.TrySetException(handle.OperationException);
                }
            };

            return tcs.Task;
        }

        protected virtual void Die()
        {
            PlayerCharactersStateMachine stateMachine = GetComponent<PlayerCharactersStateMachine>();
            stateMachine.EnterBehavior<DieState>();
        }

        public void AddObserver(IObserverByHumanoid observerByHumanoid)
        {
            observers.Add(observerByHumanoid);
            int c = observers.Count;
        }

        public void RemoveObserver(IObserverByHumanoid observerByHumanoid)
        {
            observers.Remove(observerByHumanoid);
        }

        public void NotifyObservers(object data)
        {
                _countLoaded++;
                
            if (_countLoaded==_maxCountLoaded)
            {
                foreach (var observer in observers)
                {
                    
                    OnDataLoad?.Invoke(this);
                    observer.NotifyFromHumanoid(data);
                }
            }
        }

        public void SetAudioController(AudioController audioController)
        {
            _audioController = audioController;
        }

        public AudioController GetAudioController()
        {
            return _audioController;
        }

        public void SetSelected(bool isSelected)
        {
            _isSelected = isSelected;
        }

        public void SetPontInfo()
        {
        }
    }
}