﻿using Enemies.AbstractEntity;
using Infrastructure.Logic.WeaponManagment;

namespace Enemies.Aliens
{
    public class Simple : Enemy,IDamageable
    {
        private int _levelNumber = 1;
        private int _minDamage = 30;

        public override void PushForGranade()
        {
           
        }

        public override void AdditionalDamage(float getDamage, ItemType itemItemType)
        {
            if (getDamage<_minDamage)
            {
                OnAction(EnemyEventType.TakeDamage,itemItemType);
            }
            
        }
    }
}