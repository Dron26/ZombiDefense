
using System;
using Infrastructure.Logic.WeaponManagment;

public interface IDamageable
{
    event Action<float, WeaponType> OnTakeDamage;
    void ApplyDamage(float damage, WeaponType weaponType);
}
