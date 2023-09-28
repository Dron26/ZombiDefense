using System;
using System.Collections.Generic;
using Data.Upgrades;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Infrastructure.Logic.WeaponManagment
{
    public class WeaponController : MonoCache
    {
        private GameObject _weaponPrefab;

        [SerializeField] private Weapon _weapon;
        [SerializeField] private Weapon _granade;
        [SerializeField] private Sprite shootingRadiusSprite;
        [SerializeField] private ParticleSystem _ring;

        public Action ChangeWeapon;
        public Action<Weapon> OnInitialized;
        
        public int Damage => _damage;

        private PlayerCharacterAnimController _playerCharacterAnimController;
        private Animator _animator;
        private Humanoid _humanoid;
        private Dictionary<int, float> _weaponAnimInfo = new();
        private WeaponType _weaponWeaponType;
        
        private int _damage;
        private int _maxAmmo;
        private float _reloadTime;
        private float _fireRate;
        private float _range;
        public float ReloadTime => _reloadTime;
        public bool _isShotgun;
        private bool _isGranade;

        public WeaponType WeaponWeaponType => _weaponWeaponType;
        public Weapon GetWeapon() => _weapon;
        public Weapon GetGranads() => _granade;
        
        public float GetRangeAttack() => _range;

        public Weapon GetActiveWeapon() => _weapon;

        public void SetWeapon(Transform weaponTransform) =>
            _weaponPrefab.transform.parent = weaponTransform;

        private void Awake()
        {
            _humanoid = GetComponent<Humanoid>();
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
            _animator = GetComponent<Animator>();
        }

        public void Initialize()
        {
            SetAnimInfo();
            SetWeaponParametrs();
            ChangeWeapon?.Invoke();
            SetShootingRadius();
            OnInitialized?.Invoke(_weapon);
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


        public int GetDamage() => _damage;

        public float GetSpread()
        {
            float spread = 8;
            return spread;
        }
        
        public float GetSpreadAngle() =>_weapon.SpreadAngle;

        public void SetDamage(int damage) => _damage += damage;
        public void SetUpgrade(UpgradeData upgradeData, int level)
        {
            SetDamage(upgradeData.Damage);
        }
    }

    //    Sniper Vector3(0.0289999992,-0.0839999989,0.0170000009)Vector3(347.422821,89.5062866,89.6258698)
//    shotgun Vector3(0.0179999992,-0.0500000007,-0.0189999994)Vector3(348.677673,89.4979019,89.6276169)
}