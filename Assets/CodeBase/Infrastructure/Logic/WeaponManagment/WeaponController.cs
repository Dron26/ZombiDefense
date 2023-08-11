using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Observer;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Logic.WeaponManagment
{
    public class WeaponController : MonoCache, IWeapon, IObservableWeapon
    {
        private GameObject _weaponPrefab;

        [SerializeField] private Weapon _weapon;
        [SerializeField] private Weapon _granade;
        
        [SerializeField] private Sprite shootingRadiusSprite;

        [SerializeField] private ParticleSystem _ring;
        //  [SerializeField]private GameObject _granadeGameObject;
        //  [SerializeField] private Type _garnadeType;

        private List<IObserverByWeaponController> observers = new List<IObserverByWeaponController>();
        private PlayerCharacterAnimController _playerCharacterAnimController;

        private Animator _animator;

        private Humanoid _humanoid;
        private int _totalReceivedDamage;
        private Dictionary<int, float> _weaponAnimInfo = new();
        private WeaponType _weaponWeaponType;
        private int _damage;
        private int _maxAmmo;
        private float _reloadTime;
        private float _fireRate;
        private float _range;


        public UnityAction ChangeWeapon;
        public float ReloadTime => _reloadTime;
        public bool _isShotgun;
        private bool _isGranade;
        
        public WeaponType WeaponWeaponType => _weaponWeaponType;
        public Weapon GetWeapon() => _weapon;

        public Weapon GetGranads() => _granade;


        public int DamageReceived() => _totalReceivedDamage;

        public float GetRangeAttack() => _range;

        public Weapon GetActiveWeapon() => _weapon;

        public void SetWeapon(Transform weaponTransform) =>
            _weaponPrefab.transform.parent = weaponTransform;
        
        public void SetWeaponData()
        {
            _humanoid = GetComponent<Humanoid>();
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
            _animator = GetComponent<Animator>();

            SetWeapons();
            SetAnimInfo();
            SetWeaponParametrs();
            ChangeWeapon?.Invoke();
            SetShootingRadius();
            NotifyObserverWeaponController(_weapon);
        }

        private void SetWeapons()
        {
            if (_weapon==null)
            {
                _weapon = new();
                print("Send null weapon");
            }
            else
            {
                _isGranade = _weapon.IsGranade is true and true;

                _isShotgun = _weapon.IsShotgun is true and true;
            }
        }
        
        private void SetAnimInfo()
        {
            foreach (KeyValuePair<int, float> info in _playerCharacterAnimController.GetAnimInfo())
            {
                _weaponAnimInfo.Add(info.Key, info.Value);
            }
        }

        private void SetWeaponParametrs()
        {
            _weaponWeaponType = _weapon.GetWeaponType();
            _damage = _weapon.Damage;
            _maxAmmo = _weapon.MaxAmmo;
            _range = _weapon.Range;

            if (!_isGranade)
            {
                _fireRate = _weaponAnimInfo[_playerCharacterAnimController.IsShoot];
                _reloadTime = _weaponAnimInfo[_playerCharacterAnimController.Reload];
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
        
        
        public void NotifyObserverWeaponController(Weapon weapon)
        {
            foreach (var observer in observers)
            {
                observer.NotifyFromWeaponController(weapon);
            }
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
        
        public void AddObserver(IObserverByWeaponController observerByWeapon)
        {
            observers.Add(observerByWeapon);
            int c = observers.Count;
        }

        public void RemoveObserver(IObserverByWeaponController observerByWeapon) =>
            observers.Remove(observerByWeapon);

        
        public float GetSpreadAngle() =>
            _weapon.SpreadAngle;

        public void SetDamage(int damage)
        {
            _damage += damage;
        }

        protected override void OnDisable()
        {
        }
    }

    //    Sniper Vector3(0.0289999992,-0.0839999989,0.0170000009)Vector3(347.422821,89.5062866,89.6258698)
//    shotgun Vector3(0.0179999992,-0.0500000007,-0.0189999994)Vector3(348.677673,89.4979019,89.6276169)
}