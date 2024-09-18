using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Infrastructure.AIBattle.AdditionalEquipment
{
    [CreateAssetMenu(fileName = "BaseItemData", menuName = "ItemData")]
    public class ItemData : ScriptableObject
    {
        public ItemType Type;
        public int Damage;
        public int MaxAmmo;
        public float ReloadTime;
        public float FireRate;
        public float Range;
        public AudioClip ActionClip;
        public AudioClip ReloadClip;
        public float SpreadAngle;
        public int TimeBeforeExplosion;
        public bool IsMedicine;
        public int RecoveryRate;
        public bool IsGranade;
        public int GrenadeId;
        public ParticleSystem ExplosionEffect;
        
    }
}