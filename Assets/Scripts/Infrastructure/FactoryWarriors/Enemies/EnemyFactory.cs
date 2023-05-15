using System.Collections.Generic;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using UnityEngine;

namespace Infrastructure.FactoryWarriors.Enemies
{
    public class EnemyFactory : MonoBehaviour
    {
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
    }
}