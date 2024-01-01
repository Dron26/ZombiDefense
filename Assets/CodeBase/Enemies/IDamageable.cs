
using System;
using Infrastructure.Logic.WeaponManagment;

public interface IDamageable
{
    public void ApplyDamage(float damage, WeaponType weaponType);
}
