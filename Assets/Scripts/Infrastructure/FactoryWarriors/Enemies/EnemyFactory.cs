using System.Collections.Generic;
using Enemies.AbstractEntity;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] private List<EnemyData> enemiesData;
        public Enemy CreateEnemy( Enemy enemy,Transform spawnPoint)
        {
            enemy.LoadPrefab();
            GameObject newEnemy = Instantiate(enemy.GetPrefab(), spawnPoint.position, Quaternion.identity, transform);
            var enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                return enemyComponent;
            }
            else
            {
                Debug.LogError($"Prefab {enemy.GetPrefab().name} doesn't have a component of type Enemy.");
                Destroy(enemy);
                return null;
            }
        }
    
        public EnemyData GetRandomEnemyData(int level)
        {
            var enemiesWithLevel = new List<EnemyData>();
            foreach (var enemy in enemiesData)
            {
                if (enemy.Level == level)
                {
                    enemiesWithLevel.Add(enemy);
                }
            }
    
            if (enemiesWithLevel.Count > 0)
            {
                var index = Random.Range(0, enemiesWithLevel.Count);
                return enemiesWithLevel[index];
            }
            else
            {
                Debug.LogError($"No enemies found with level {level}.");
                return null;
            }
        }
    }
    
}