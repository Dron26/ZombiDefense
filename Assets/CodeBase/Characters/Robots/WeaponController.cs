using Characters.Humanoids.AbstractLevel;
using Data.Upgrades;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Characters.Robots
{
    public abstract class WeaponController : MonoCache,IWeaponController
    {
        public ItemType ItemType;
        private SpriteRenderer Radius;
        public Light Light;
        public ItemData ItemData;
        public virtual void Initialize(CharacterData data){}

        public virtual void SetPoint(WorkPoint workPoint){}

        public virtual void SetSelected(bool isSelected){}
    }
}