using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services;
using Services.SaveLoad;
using TMPro;
using UnityEngine;

namespace UI.Buttons
{
    public class CountEnemyPanel : MonoCache
    {
        [SerializeField] private TMP_Text _text;
        private EnemyHandler _enemyHandler;
        private GameEventBroadcaster _eventBroadcaster;
        private int _countEnemy;
        
        public void Initialize()
        {
            _enemyHandler=AllServices.Container.Single<EnemyHandler>(); 
            _eventBroadcaster=AllServices.Container.Single<GameEventBroadcaster>(); 
            _eventBroadcaster.OnEnemyDeath += OnEnemyDeath;
            SetCount(_enemyHandler.GetActiveEnemy().Count);
        }

        private void SetCount(int count)
        {
            _countEnemy = count;
            _text.text=_countEnemy.ToString();
        }
        
        private void OnEnemyDeath(Enemy enemy)
        {
            _countEnemy--;
            SetCount(_countEnemy);
        }
        
    }
}