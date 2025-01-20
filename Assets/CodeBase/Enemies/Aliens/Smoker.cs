using Enemies.AbstractEntity;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Enemies.Aliens
{
    public class Smoker : Enemy,IDamageable
    {

        private int _levelNumber = 4;
        [SerializeField] private int _bombDamage;
        private int _minDamage = 30;
        [SerializeField] private int _explosionRadius = 4;

        public override void PushForGranade()
        {
        
        }

        public override void AdditionalDamage(float getDamage, ItemType itemItemType)
        {
            if ( itemItemType == ItemType.Grenade)
            {
                OnAction(EnemyEventType.TakeGranadeDamage,itemItemType);
            }
        }
    }
}