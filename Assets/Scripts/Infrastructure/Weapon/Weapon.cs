using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Infrastructure.Weapon
{
    public  class Weapon:MonoCache
    {
        public Type WeaponTypeEnum;
        [SerializeField] private string _weaponName;
        [SerializeField] private int _damage;
        [SerializeField] private int _maxAmmo;
        [SerializeField] private float _reloadTime;
        [SerializeField] private float _fireRate;
        [SerializeField] private float _range;
        [SerializeField] public GameObject WeaponPrefab;
        public string WeaponName => _weaponName;
        public int Damage => _damage;
        public int MaxAmmo => _maxAmmo;
        public float ReloadTime => _reloadTime;
        public float FireRate => _fireRate;
        public float Range => _range;

        public Vector3 GetWweaponPoint()
        {
          return WeaponPrefab.gameObject.transform.position;
        } 
    }
    
    public enum Type
    {
        Pistol = 1,
        Rifle = 2,
        SniperRifle = 3,
        Grenade = 4,
    }
}