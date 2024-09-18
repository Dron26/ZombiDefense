using Enemies.AbstractEntity;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    
    private float _sourceVolume;
    
    public void ExecuteExplosion(Vector3 explosionPosition, float explosionRadius, int damage, ParticleSystem explosionEffect,float volumeAudio)
    {
        _sourceVolume = volumeAudio;
        // Создание эффекта взрыва
        GameObject exploded = Instantiate(explosionEffect.gameObject, explosionPosition, Quaternion.identity);
        exploded.GetComponent<ParticleSystem>().Play();
        // Используем слой "Enemies" для поиска врагов
        int enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius, enemyLayer);

        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(collider.transform.position, explosionPosition);
            float damagePercentage = Mathf.Clamp01(1 - distance / explosionRadius);
            int calculatedDamage = Mathf.RoundToInt(damagePercentage * damage);

            // Применяем урон, если найден враг
            if (collider.TryGetComponent(out Enemy currentEnemy))
            {
                IDamageable damageable = collider.GetComponent<IDamageable>();
                damageable?.ApplyDamage(calculatedDamage, ItemType.Grenade);
            }
        }
    }
}