using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Infrastructure.Logic.WeaponManagment
{
    public  class Weapon:MonoCache
    {
        [SerializeField] private WeaponType _weaponWeaponTypeEnum;
        [SerializeField] private int _damage;
        [SerializeField] private int _maxAmmo;
        [SerializeField] private float _reloadTime;
        [SerializeField] private float _fireRate;
        [SerializeField] private float _range;
        [SerializeField] private AudioClip _shoot;
        [SerializeField] private float _spreadAngle;
        [SerializeField] private AudioClip _reload;
        
        public AudioClip Shoot => _shoot;
        public AudioClip Reload => _reload;
        public int Damage => _damage;
        public int MaxAmmo => _maxAmmo;
        public float ReloadTime => _reloadTime;
        public float FireRate => _fireRate;
        public float Range => _range;
        public bool IsShotgun => _weaponWeaponTypeEnum == WeaponType.Shotgun;
        public bool IsGranade => _weaponWeaponTypeEnum == WeaponType.Grenade;
        
        public float SpreadAngle=>_spreadAngle;
        
        public WeaponType GetWeaponType() => _weaponWeaponTypeEnum;
    }
}