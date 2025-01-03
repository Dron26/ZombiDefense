using Characters.Humanoids.AbstractLevel;
using Enemies.AbstractEntity;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

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
        if (Level == _levelNumber && _minDamage>=getDamage)
        {
            OnAction(EnemyEventType.TakeSmokerDamage,itemItemType);
        }
        else if(getDamage>=_minDamage)
        {
            
            Vector3 explosionPosition=transform.position;
            float explosionRadius=0;
            float maxDamage=0;
        
            // Используем слой "Enemies" для поиска только вражеских объектов
            int enemyLayer = LayerMask.GetMask("Player");
            Collider[] colliders = Physics.OverlapSphere(explosionPosition, _explosionRadius, enemyLayer);

            foreach (Collider collider in colliders)
            {
                
                
                float distance = Vector3.Distance(collider.transform.position, explosionPosition);
                float damagePercentage = Mathf.Clamp01(1 - distance / _explosionRadius);
                int calculatedDamage = Mathf.RoundToInt(damagePercentage * _bombDamage);
                
                // Тот же код для нанесения урона
                if (collider.TryGetComponent( out Humanoid humanoid))
                {
                    humanoid.gameObject.transform.LookAt(transform.position);
                    humanoid.ApplyDamage(calculatedDamage,itemItemType);
                }

                ;
                IDamageable damageable = collider.GetComponent<IDamageable>();
            
                if (damageable != null)
                {
                    // Рассчет урона и вызов TakeDamage
                    // ...
                }
            }


            Debug.Log("Бабах");
            Destroy(gameObject,2);
              
        }
    }
    void IDamageable.ApplyDamage(float damage, ItemType itemType)
    {
    }
}