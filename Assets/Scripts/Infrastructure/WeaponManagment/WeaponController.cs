using System.Collections.Generic;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Observer;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.WeaponManagment
{
    public class WeaponController : MonoCache, IWeapon, IObserverByHumanoid,IObservableWeapon
    {
        private WeaponData _weaponData;
        private GameObject _weaponPrefab;
        
        private Weapon _smallArms;
        private Weapon _granade;
        
         [SerializeField]private GameObject _smallArmsGameObject;
        [SerializeField] private Type _smallArmsType;
        [SerializeField] private Sprite shootingRadiusSprite;

        [SerializeField ]private ParticleSystem _ring;
        //  [SerializeField]private GameObject _granadeGameObject;
        //  [SerializeField] private Type _garnadeType;
         
        private List<IObserverByWeaponController> observers = new List<IObserverByWeaponController>();
        private PlayerCharacterAnimController _playerCharacterAnimController;
        private Animator _animator;
        //private  Weapon _weaponGranade;
        private int _numberSmallArms = 0;
        private int _numberGranade = 4;
        private Humanoid _humanoid;
        private float _fireTimer = 0f;
        private int _totalReceivedDamage;
        private Dictionary<int, float> _weaponAnimInfo=new();
        private string _weaponName;
        private int _damage;
        private int _maxAmmo;
        private float _reloadTime;
        private float _fireRate;
        private float _range;
    

        public UnityAction ChangeWeapon;
        public float ReloadTime => _reloadTime;
        public bool _isShotgun;
        public string WeaponName=>_weaponName;
        private void Awake()
        {
            if (TryGetComponent(out Humanoid humanoid))
            {
                humanoid.AddObserver(this);
            }
            else if (TryGetComponent(out Enemy enemy))
            {
                enemy.AddObserver(this);
            }
        }
        
        
        public void SetSmallArmsPrefab()
        {
            _smallArmsGameObject.SetActive(true);
            
        }
        
        // public void SetGranadePrefab()
        // {
        //     _granadeGameObject.SetActive(true);
        //     
        // }
        
        public WeaponData GetWeaponData()
        {
            return _weaponData;
        }
        
        public Weapon GetSmallArms()
        {
            return _smallArms;
        }
        
        public Weapon GetGranads()
        {
            return _granade;
        }

        private void SetWeapons()
        { 
            List<Weapon> smallArms=_weaponData.SmallArms();
         //   List<Weapon> granads=_weaponData.Granads();

            if (smallArms!=null)
            {
                for (int i = 0; i < smallArms.Count; i++)
                {
                    if (smallArms[i]!=null)
                    {
                        _smallArms=smallArms[i];
                        _isShotgun=_smallArms.IsShotgun;
                    }
                }
            }
            else
            {
                _smallArms=new ();
                print("Send null smallArms in Prefab");
            }
           
            // if (granads != null)
            // {
            //     for (int i = 0; i < granads.Count; i++)
            //     {
            //         if (granads[i]!=null)
            //         {
            //             _granade=granads[i];
            //         }
            //     }
            // }
            // else
            // {
            //     granads=new ();
            //     print("Send null granads in Prefab");
            // }
            
            
            
        }

        private void SetWeaponParametrs()
        {
            _weaponName = _smallArms.WeaponName;
            _damage = _smallArms.Damage;
            _maxAmmo = _smallArms.MaxAmmo;
            _fireRate = _weaponAnimInfo[_playerCharacterAnimController.IsShoot];
            _reloadTime = _weaponAnimInfo[_playerCharacterAnimController.Reload];
            _range = _smallArms.Range;
        }
        
        

        public int GetDamage()
        {
            _totalReceivedDamage += _damage;
            return _damage;
        }
        
        public float GetSpread()
        {
            float spread = 8;

            return spread;
        }
        

        public int DamageReceived() =>
            _totalReceivedDamage;

        public float GetRangeAttack()
        {
            return _range;
        }

        public Weapon GetActiveWeapon()
        {
            return _smallArms;
        }
        public void SetWeapon(Transform weaponTransform )
        {
            _weaponPrefab.transform.parent = weaponTransform;
        }

        private void SetAnimInfo()
        {
            foreach (KeyValuePair<int, float> info in _playerCharacterAnimController.GetAnimInfo())
            {
                _weaponAnimInfo.Add(info.Key, info.Value);
            }
        }

        public void NotifyFromHumanoid(object data)
        {
            _humanoid=GetComponent<Humanoid>();
            _weaponData = _humanoid.GetWeaponData();
          //  _humanoid.OnHumanoidSelected=NotifySelection;
            
            _playerCharacterAnimController=GetComponent<PlayerCharacterAnimController>();
            _animator=GetComponent<Animator>();

            SetWeapons();
            SetAnimInfo();
            SetWeaponParametrs();
            SetSmallArmsPrefab();
            ChangeWeapon?.Invoke();
            SetShootingRadius();
            NotifyObserverWeaponController(_smallArms);
        }

        public void NotifySelection(bool isSelected)
        {
            if (isSelected)
            {
                _ring.Play();
            }
            else
            {
                _ring.Stop();
            }
        }

        private void SetShootingRadius()
        {
            GameObject radiusObject = new GameObject("ShootingRadius");
            radiusObject.SetActive(true);
            radiusObject.transform.position = transform.position;
            SpriteRenderer spriteRenderer = radiusObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = shootingRadiusSprite;
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            radiusObject.transform.localScale = new Vector3(_range * 2f, _range * 2f, 1f);
            radiusObject.SetActive(false);
        }


        private void OnDisable()
        {
            if (TryGetComponent(out Humanoid humanoid))
            {
                humanoid.RemoveObserver(this);
            }
            else if (TryGetComponent(out Enemy enemy))
            {
                enemy.RemoveObserver(this);
            }
        }

        public void AddObserver(IObserverByWeaponController observerByWeapon)
        {
            observers.Add(observerByWeapon);
            int c= observers.Count;
        }

        public void RemoveObserver(IObserverByWeaponController observerByWeapon)
        {
            observers.Remove(observerByWeapon);
        }

        public void NotifyObserverWeaponController(Weapon weapon)
        {
            foreach (var observer in observers)
            {
                observer.NotifyFromWeaponController(weapon);
            }
        }

        public float GetSpreadAngle() => 
            _smallArms.SpreadAngle;
    }

    //    Sniper Vector3(0.0289999992,-0.0839999989,0.0170000009)Vector3(347.422821,89.5062866,89.6258698)
//    shotgun Vector3(0.0179999992,-0.0500000007,-0.0189999994)Vector3(348.677673,89.4979019,89.6276169)
}