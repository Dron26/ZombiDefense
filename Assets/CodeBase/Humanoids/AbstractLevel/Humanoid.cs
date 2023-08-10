using System.Collections.Generic;
using System.Threading.Tasks;
using Audio;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Humanoids;
using Infrastructure.WeaponManagment;
using Observer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using Upgrades;

namespace Humanoids.AbstractLevel
{
    [RequireComponent(typeof(WeaponController))]
    [RequireComponent(typeof(PlayerCharactersStateMachine))]
    public abstract class Humanoid : MonoCache, IObservableHumanoid
    {
        [SerializeField]  private int _id;
        [SerializeField]  private string _name;
        [SerializeField] private string _characterInfoName;
        
        public Vector3 StartPosition;
        private int _countLoaded = 0;
        private int _maxCountLoaded = 2;
        private AudioManager _audioManager;
        private List<IObserverByHumanoid> observers = new List<IObserverByHumanoid>();
        public UnityAction<Humanoid> OnDataLoad;
        public UnityAction<bool> OnHumanoidSelected;
        [SerializeField ]private ParticleSystem _ring;
        private WeaponController _weaponControl;
       
        public int ID=>_id;
        protected HumanoidData humanoidData;
        private bool _isBuyed = false;
        public bool IsBuyed => _isBuyed;
        public bool IsSelected => _isSelected;
        public abstract int GetLevel();
        public abstract int GetHealth();
        public abstract int GetMaxHealth();
        public abstract bool IsLife();
        public abstract int GetPrice();
        public bool IsMove=>_isMoving;
        public abstract int GetDamageDone();
        public UnityAction OnMove;
        public UnityAction OnLoadData;
        private bool _isSelected;
        private bool _isMoving;
public string GetName() => _name;
        public abstract void ApplyDamage(int getDamage);
        public abstract Sprite GetSprite();

        public void Initialize()
        {
            GetWeaponController().SetWeaponData();
            NotifyObservers(this);
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
                foreach (var observer in observers)
                {
                    OnDataLoad?.Invoke(this);
                    observer.NotifyFromHumanoid(data);
                }
        }

        public void SetAudioController(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        public AudioManager GetAudioController()
        {
            return _audioManager;
        }

        public void SetSelected(bool isSelected)
        {
            _isSelected = isSelected;
            
            if(_isSelected==true)
            {
                _ring.gameObject.SetActive(true);
            }
            else
            {
                _ring.gameObject.SetActive(false);
            }
        }
        

        public void SetPontInfo()
        {
        }

        public void IsMoving(bool isMoving)
        {
            _isMoving = isMoving;
            OnMove?.Invoke();
        }


        public void SetAvailable(bool isBuyed)
        {
            isBuyed=isBuyed;
        }

        public WeaponController GetWeaponController()
        {
            _weaponControl= GetComponent<WeaponController>();    
            return _weaponControl;
        }

        public abstract void SetUpgrade(UpgradeData upgrade, int level);

        public string GetInfoName() => _characterInfoName;
    }
}