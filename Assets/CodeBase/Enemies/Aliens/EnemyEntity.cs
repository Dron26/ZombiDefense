using Characters.Humanoids.AbstractLevel;
using Enemies;
using Enemies.AbstractEntity;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

public class EnemyEntity : Enemy,IDamageable
{
    private int _minDamage = 30;

    public override void PushForGranade()
    {
           
    }

    public override void AdditionalDamage(float getDamage, ItemType itemItemType)
    {
        if (_minDamage>=getDamage)
        {
            OnAction(EnemyEventType.TakeSimpleWalkerDamage,itemItemType);
        }
        if (Data.Type==EnemyType.Smoker && _minDamage>=getDamage)
        {
            OnAction(EnemyEventType.TakeSmokerDamage,itemItemType);
        }
        else if(getDamage>=_minDamage)
        {
           
            
            

            Debug.Log("Бабах");
            Destroy(gameObject,2);
        }
    }
}