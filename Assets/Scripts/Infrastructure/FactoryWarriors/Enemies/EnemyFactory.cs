using System.Collections.Generic;
using System.Threading.Tasks;
using Audio;
using Cysharp.Threading.Tasks;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.FactoryWarriors.Enemies
{
    public class EnemyFactory : MonoBehaviour
    {
        public UnityAction<Enemy> CreatedEnemy;
        public async Task<GameObject> Create(GameObject enemy)
        {
            GameObject newEnemy = Instantiate(enemy);
            Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
            enemyComponent.OnDataLoad = Created;
            
            await UniTask.SwitchToMainThread();
            await enemyComponent.LoadPrefab();
            
            return newEnemy;
        }

        private void Created(Enemy enemyComponent)
        {
            CreatedEnemy?.Invoke(enemyComponent);
        }
    }
}