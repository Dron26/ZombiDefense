using System;
using Enemies.AbstractEntity;
using Infrastructure.Logic.WeaponManagment;

namespace Enemies.Aliens
{
    public class SimpleWalker : Enemy
    {
        private int _levelNumber = 4;
        private int _minDamage = 30;
        
        public override void AdditionalDamage(float getDamage, WeaponType weaponWeaponType)
        {
            if (Level == _levelNumber && getDamage > _minDamage)
            {
                OnAction(EnemyEventType.TakeSimpleWalkerDamage);
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
        if (Level == _levelNumber && getDamage > _minDamage)
        {
            OnAction(EnemyEventType.TakeSmokerDamage);
        }
    }
}