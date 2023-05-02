using System;
using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Unity.VisualScripting;
using UnityEngine;

namespace Infrastructure.Weapon
{
    public class WeaponController : MonoCache, IWeapon
    {
        private WeaponData _weaponData;
        private GameObject _weaponPrefab;
        private List<Weapon> _weapons;
        
        private Weapon _weaponSmallArms;

        //private  Weapon _weaponGranade;
        private int _numberSmallArms = 3;
        private int _numberGranade = 4;
        private Humanoid _humanoid;
        private float _fireTimer = 0f;
        private int _ammoCount;
        private int _totalReceivedDamage;


        private string _weaponName;
        private int _damage;
        private int _maxAmmo;
        private float _reloadTime;
        private float _fireRate;
        private float _range;

        public Vector3 Position;
        public void Initialize()
        {
             _humanoid=GetComponent<Humanoid>();
            _weaponData = _humanoid.GetWeaponData();
            _weapons = _weaponData.Weapons;
            CheckOfType();
            SetParametr();
        }

        public void SetParametr()
        {
            _weaponName = _weaponSmallArms.WeaponName;
            _damage = _weaponSmallArms.Damage;
            _maxAmmo = _weaponSmallArms.MaxAmmo;
            _reloadTime = _weaponSmallArms.ReloadTime;
            _fireRate = _weaponSmallArms.FireRate;
            _range = _weaponSmallArms.Range;
            
            GameObject emptyObject =  new GameObject();
            emptyObject.transform.position = _weaponSmallArms.GetWweaponPoint();
            GameObject effectInstance = Instantiate(_weaponSmallArms.WeaponPrefab);
            effectInstance.transform.SetParent(emptyObject.transform);
            Destroy(emptyObject);
            
        }
        protected  void Update()
        {
            if (_fireTimer > 0)
            {
                _fireTimer -= Time.deltaTime;
            }
        }

        public bool CanFire()
        {
            return _fireTimer <= 0 && _ammoCount > 0;
        }

        public void Fire()
        {
            if (!CanFire())
            {
                return;
            }

            // TODO: реализуйте стрельбу по объекту в радиусе поражения оружия

            _ammoCount--;

            if (_ammoCount <= 0)
            {
                Reload();
            }
            else
            {
                _fireTimer = 1f / _fireRate;
            }
        }

        public void Reload()
        {
            _ammoCount = _maxAmmo;
            _fireTimer = _reloadTime;
        }

        public WeaponData GetWeaponData()
        {
            return _weaponData;
        }

        private void CheckOfType()
        {
            foreach (Weapon weapon in _weapons)
            {
                if ((int)weapon.WeaponTypeEnum <= _numberSmallArms)
                {
                    _weaponSmallArms = weapon;
                }
                // else if ((int)weapon.WeaponTypeEnum<=_numberGranade)
                // {
                //     _weaponGranade=weapon;
                // }
            }
        }

        public int GetDamage()
        {
            _totalReceivedDamage += _damage;
            return _damage;
        }

        public int DamageReceived() =>
            _totalReceivedDamage;

        public float GetRangeAttack()
        {
            return _range;
        }

        public void SetWeapon(Transform weaponTransform )
        {
            _weaponPrefab.transform.parent = weaponTransform;
        }
    }

    interface IWeapon
    {
         abstract int GetDamage();
         abstract float GetRangeAttack();
    }
}