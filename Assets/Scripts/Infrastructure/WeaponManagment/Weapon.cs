using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Infrastructure.WeaponManagment
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
        [SerializeField] private AudioClip _shoot;

        [SerializeField] private AudioClip _reload;
        
        public AudioClip Shoot => _shoot;
        public AudioClip Reload => _reload;
        public string WeaponName => _weaponName;
        public int Damage => _damage;
        public int MaxAmmo => _maxAmmo;
        public float ReloadTime => _reloadTime;
        public float FireRate => _fireRate;
        public float Range => _range;
        public bool IsShotgun => WeaponTypeEnum == Type.Shotgun;
    }
    
    public enum Type
    {
        Pistol = 0,
        Revolver= 1,
        Rifle = 2,
        SniperRifle = 3,
        Grenade = 4,
        HeavyMachineGun = 5,
            Shotgun = 6}
}