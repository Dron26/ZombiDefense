using Data.Upgrades;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;using UnityEngine;

namespace Infrastructure.Logic.WeaponManagment
{
    public abstract class WeaponController : MonoCache,IWeaponController
    {
        public ItemType ItemType;
        private SpriteRenderer Radius;
        public Light Light;
        public ItemData ItemData;
        public int Damage { get; set; }
        
        public void SetUpgrade(UpgradeData upgradeData, int level) => SetDamage(upgradeData.Damage);

        public virtual void Initialize(){}

        public virtual void SetPoint(WorkPoint workPoint){}

        public virtual void SetSelected(bool isSelected){}
        private void SetDamage(int damage) => Damage += damage;
    }
}