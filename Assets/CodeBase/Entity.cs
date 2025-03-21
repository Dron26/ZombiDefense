using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;

namespace Enemies.AbstractEntity
{
    public abstract class Entity : MonoCache
    {
        public event Action<Entity> OnEntityDeath;
    
        protected void RaiseEntityEvent()
        {
            OnEntityDeath?.Invoke(this);
        }

        public abstract bool IsLife();
    
        public abstract void ApplyDamage(float damage, ItemType itemType);
    }
}