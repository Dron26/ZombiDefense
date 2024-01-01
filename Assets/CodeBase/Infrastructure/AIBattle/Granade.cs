using System;
using System.Collections;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    public class Granade : MonoCache
    {
        [SerializeField] private float _time;
        private WeaponType _weaponType;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private int _damage;
        [SerializeField] private ParticleSystem _explosion;
        private Action<bool> isExploded;

        private void Explosion()
        {
            GameObject exploded = Instantiate(_explosion.gameObject, transform.position, Quaternion.identity);
            exploded.GetComponent<ParticleSystem>().Play();
            
            Vector3 explosionPosition = transform.position;
            Debug.Log("точка взрыва explosionPosition");

            // Используем слой "Enemies" для поиска только вражеских объектов
            int enemyLayer = LayerMask.GetMask("Enemy");
            Collider[] colliders = Physics.OverlapSphere(explosionPosition, _explosionRadius, enemyLayer);

            foreach (Collider collider in colliders)
            {
                float distance = Vector3.Distance(collider.transform.position, explosionPosition);
                float damagePercentage = Mathf.Clamp01(1 - distance / _explosionRadius);
                int calculatedDamage = Mathf.RoundToInt(damagePercentage * _damage);

                // Тот же код для нанесения урона
                if (collider.TryGetComponent(out Enemy currentEnemy))
                {
                }

                ;
                IDamageable damageable = collider.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    currentEnemy.gameObject.transform.LookAt(transform.position);
                    currentEnemy.ApplyDamage(calculatedDamage, _weaponType);
                }
            }


            Debug.Log("Бабах");
            isExploded?.Invoke(true);
            Destroy(gameObject);
        }

        private IEnumerator ThrowerControlling()
        {
            while (_time! > 0)
            {
                _time -= Time.deltaTime;
                yield return null;
            }

            Explosion();
        }

        public void Work()
        {
            _weaponType = WeaponType.Grenade;
            StartCoroutine(ThrowerControlling());
        }
    }
}