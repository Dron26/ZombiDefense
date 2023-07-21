using System.Collections.Generic;
using System.Threading.Tasks;
using Audio;
using Cysharp.Threading.Tasks;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.FactoryWarriors.Enemies
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
            print("Enemy created");
        }
    }
}