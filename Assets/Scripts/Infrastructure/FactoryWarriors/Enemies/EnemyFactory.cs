using System.Collections.Generic;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] private List<EnemyData> enemiesData;
        private static readonly List<Humanoid> _humanoids = new();
        private static readonly List<Enemy> _enemies = new();
        public Enemy Create( GameObject enemy)
        {
            
            GameObject newEnemy = Instantiate(enemy,transform);
            newEnemy.gameObject.SetActive(false);
            Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
            enemyComponent.LoadPrefab();
            _enemies.Add(enemyComponent);
            
            if (enemyComponent != null)
            {
                return enemyComponent;
            }
            else
            {
                Debug.LogError($"PrefabCharacter {enemyComponent.GetPrefab().name} doesn't have a component of type Enemys.");
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
        public void SetHumanoidData(List<Humanoid> humanoid)
        {
            foreach (Humanoid tempHumanoid in humanoid)
            {
                _humanoids.Add(tempHumanoid);
            }
        }
        public List<Humanoid> GetAllHumanoids =>
            _humanoids;

        public List<Enemy> GetAllEnemies => 
            _enemies;

        
    }
    
}