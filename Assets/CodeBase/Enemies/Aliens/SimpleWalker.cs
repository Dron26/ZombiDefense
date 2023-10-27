using System;
using Enemies.AbstractEntity;
using Infrastructure.Logic.WeaponManagment;

namespace Enemies.Aliens
{
    public class SimpleWalker : Enemy
    {
        private int _levelNumber = 1;
        private int _minDamage = 30;
        
        public override void AdditionalDamage(float getDamage, WeaponType weaponWeaponType)
        {
            if (Level == _levelNumber && _minDamage>=getDamage)
            {
                OnAction(EnemyEventType.TakeSimpleWalkerDamage,weaponWeaponType);
            }
        }
    }
}

public class Smoker : Enemy
{

    private int _levelNumber = 4;
    private int _minDamage = 30;
    
    public override void AdditionalDamage(float getDamage, WeaponType weaponWeaponType)
    {
        if (Level == _levelNumber && _minDamage>=getDamage)
        {
            OnAction(EnemyEventType.TakeSmokerDamage,weaponWeaponType);
        }
    }
}