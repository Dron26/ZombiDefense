using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Factories.FactoryWarriors.Enemies
{
    public class EnemyFactory : MonoCache
    {
        public UnityAction<Enemy> CreatedEnemy;
        
        public void Create(GameObject enemy)
        {
            GameObject newEnemy = Instantiate(enemy);
            Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
            enemyComponent.OnDataLoad = Created; 
            enemyComponent.LoadPrefab();
        }

        private void Created(Enemy enemyComponent)
        {
            CreatedEnemy?.Invoke(enemyComponent);
        }
    }
}