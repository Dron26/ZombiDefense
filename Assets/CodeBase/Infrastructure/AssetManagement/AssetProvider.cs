using Enemies.AbstractEntity;
using UnityEngine;
using System.Collections.Generic;

namespace Infrastructure.AssetManagement
{
    public class AssetProvider : IAssets
    {
        private Dictionary<string, GameObject> _cache = new Dictionary<string, GameObject>();
        
        public GameObject Instantiate(string path)
        {
            if (_cache.ContainsKey(path))
            {
                return Object.Instantiate(_cache[path]);
            }
            
            var prefab = Resources.Load<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogError($"Failed to load GameObject at path: {path}. Check if the path is correct and the asset exists.");
                return null;
            }
            
            _cache[path] = prefab;
            return Object.Instantiate(prefab);
        }

        public GameObject Instantiate(string path, Transform parent)
        {
            var instance = Instantiate(path);
            if (instance != null && parent != null)
            {
                instance.transform.SetParent(parent, false);
            }
            return instance;
        }

        public Enemy LoadEnemy(string path)
        {
            var enemyPrefab = Resources.Load<Enemy>(path);
            if (enemyPrefab == null)
            {
                Debug.LogError($"Failed to load Enemy at path: {path}. Check if the path is correct and the asset exists.");
                return null;
            }
            return Object.Instantiate(enemyPrefab);
        }
    }
}