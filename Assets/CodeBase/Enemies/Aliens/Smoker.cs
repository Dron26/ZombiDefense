using Enemies.AbstractEntity;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Enemies.Aliens
{
    public class Smoker : Enemy,IDamageable
    {

        [SerializeField] private int _bombDamage;

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