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
        private IEnemyHandler _enemyHandler;
        private IGameEventBroadcaster _eventBroadcaster;
        private int _countEnemy;
        
        public void Initialize()
        {
            _enemyHandler=AllServices.Container.Single<IEnemyHandler>(); 
            _eventBroadcaster=AllServices.Container.Single<IGameEventBroadcaster>();
            AddListener();
            SetCount(_enemyHandler.GetMaxEnemyOnWave());
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
        
        private void AddListener()
        {
            _eventBroadcaster.OnEnemyDeath += OnEnemyDeath;
            _eventBroadcaster.OnOnSetMaxEnemy += SetCount;
        }
        
        private void OnDestroy()
        {
            RemoveListener();
        }

        private void RemoveListener()
        {
            _eventBroadcaster.OnEnemyDeath-= OnEnemyDeath;
            _eventBroadcaster.OnOnSetMaxEnemy -= SetCount;
        }
        
    }
}