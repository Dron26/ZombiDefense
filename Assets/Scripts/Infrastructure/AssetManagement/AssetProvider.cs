using Enemies.AbstractEntity;
using UnityEngine;

namespace Infrastructure.AssetManagement
{
    public class AssetProvider : IAssets, IEnemyAssets
    {
        public GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        public GameObject Instantiate(string path, Transform transform)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab,transform);
        }

        public Enemy LoadEnemy(string path)
        {
            var enemyTypePrefab = Resources.Load<Enemy>(path);
            return Object.Instantiate(enemyTypePrefab);
        }
    }
}