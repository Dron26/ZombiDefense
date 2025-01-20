using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class DamageZonePool : MonoBehaviour
    {
        public static DamageZonePool Instance { get; private set; }

        [Header("Pool Settings")]
        [SerializeField] private GameObject _damageZonePrefab;
        [SerializeField] private int _poolSize = 10;

        private Queue<DamageZone> _pool = new Queue<DamageZone>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                InitializePool();
            }
            else
            {
                Debug.LogWarning("Multiple instances of DamageZonePool detected. Destroying the new one.");
                Destroy(gameObject);
            }
        }

        private void InitializePool()
        {
            if (_damageZonePrefab == null)
            {
                Debug.LogError("DamageZone prefab is not assigned in the inspector!");
                return;
            }

            for (int i = 0; i < _poolSize; i++)
            {
                CreateAndEnqueueDamageZone();
            }
        }

        private DamageZone CreateAndEnqueueDamageZone()
        {
            var damageZone = Instantiate(_damageZonePrefab).GetComponent<DamageZone>();
            if (damageZone == null)
            {
                Debug.LogError("DamageZone prefab does not have a DamageZone component!");
                return null;
            }

            damageZone.gameObject.SetActive(false);
            _pool.Enqueue(damageZone);
            return damageZone;
        }

        public DamageZone Get()
        {
            if (_pool.Count == 0)
            {
                Debug.LogWarning("Pool is empty! Consider increasing pool size.");
                return CreateAndEnqueueDamageZone(); // Теперь метод возвращает DamageZone
            }

            var damageZone = _pool.Dequeue();
            damageZone.gameObject.SetActive(true);
            return damageZone;
        }

        public void Return(DamageZone damageZone)
        {
            if (damageZone == null)
            {
                Debug.LogWarning("Attempted to return a null DamageZone to the pool.");
                return;
            }

            damageZone.gameObject.SetActive(false);
            _pool.Enqueue(damageZone);
        }
    }
}