using Enemies;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    public class ExplosionManager : MonoBehaviour
    {
    
        private float _sourceVolume;
    
        public void ExecuteExplosion(Vector3 explosionPosition, float explosionRadius, int damage, ParticleSystem explosionEffect,float volumeAudio)
        {
            _sourceVolume = volumeAudio;
            GameObject exploded = Instantiate(explosionEffect.gameObject, explosionPosition, Quaternion.identity);
            exploded.GetComponent<ParticleSystem>().Play();
            int enemyLayer = LayerMask.GetMask("Character");
            Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius, enemyLayer);

            foreach (Collider collider in colliders)
            {
                float distance = Vector3.Distance(collider.transform.position, explosionPosition);
                float damagePercentage = Mathf.Clamp01(1 - distance / explosionRadius);
                int calculatedDamage = Mathf.RoundToInt(damagePercentage * damage);

                if (collider.TryGetComponent(out IDamageable damageable))
                {
                    damageable.ApplyDamage(calculatedDamage, ItemType.Grenade);
                }
            }
        }
    }
}