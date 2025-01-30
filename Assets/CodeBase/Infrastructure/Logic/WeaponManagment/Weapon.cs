using Infrastructure.AIBattle.AdditionalEquipment;
using UnityEngine;

namespace Infrastructure.Logic.WeaponManagment
{
    public  class Weapon : WeaponItem
    {
        private ItemData _itemData;
        [SerializeField] private  ParticleSystem _particleGunshot;
        [SerializeField] private  ParticleSystem _particleEject;
        [SerializeField] private  Light _light;

        public override int Damage => _itemData.Damage;
        public override float Range => _itemData.Range;
        public float ReloadTime => _itemData.ReloadTime;
        public float FireRate => _itemData.FireRate;
        public AudioClip ActionClip => _itemData.ActionClip;
        public AudioClip ReloadClip => _itemData.ReloadClip;
        public int MaxAmmo => _itemData.MaxAmmo;
        public int TimeBeforeExplosion => _itemData.TimeBeforeExplosion;
        public float SpreadAngle=>_itemData.SpreadAngle;

        public ParticleSystem GetParticleGunshot=>_particleGunshot;
        public ParticleSystem GetParticleEject=>_particleEject;
        public Light Light=>_light;

        public override ItemType ItemType => _itemData.Type;
        public override void Initialize(ItemData itemData) => _itemData = itemData;
    }
}