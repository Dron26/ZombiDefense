using System;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

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