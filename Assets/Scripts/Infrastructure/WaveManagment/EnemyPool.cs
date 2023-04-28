using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.WaveManagment
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int initialPoolSize = 10;
        [SerializeField] private bool canGrow = true;

        private readonly Queue<GameObject> pool = new Queue<GameObject>();

        private void Awake()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                var obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
        }

        public GameObject GetObject()
        {
            GameObject obj;

            if (pool.Count > 0)
            {
                obj = pool.Dequeue();
                obj.SetActive(true);
            }
            else if (canGrow)
            {
                obj = Instantiate(prefab, transform);
            }
            else
            {
                obj = null;
            }

            return obj;
        }

        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}