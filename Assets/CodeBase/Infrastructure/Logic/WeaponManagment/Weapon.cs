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
        [SerializeField] private AudioClip _actionClip;
        [SerializeField] private float _spreadAngle;
        [SerializeField] private AudioClip _reloadClip;
        [SerializeField] private int IdGranade;
        [SerializeField] private int  _timeBeforeExplosion;
        
        public AudioClip ActionClip => _actionClip;
        public AudioClip ReloadClip => _reloadClip;
        public int Damage => _damage;
        public int MaxAmmo => _maxAmmo;
        public float ReloadTime => _reloadTime;
        public float FireRate => _fireRate;
        public float Range => _range;
        public bool IsShotgun => _weaponWeaponTypeEnum == WeaponType.Shotgun;
        public bool IsGranade => _weaponWeaponTypeEnum == WeaponType.Grenade;
        
        public float SpreadAngle=>_spreadAngle;
        
        public WeaponType GetWeaponType() => _weaponWeaponTypeEnum;
        
        public int GetIdGranade() => IdGranade;
        
        public int GetTimeBeforeExplosion() => _timeBeforeExplosion;
    }
}