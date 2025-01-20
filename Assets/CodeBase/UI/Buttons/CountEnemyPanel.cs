using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Services.SaveLoad;
using TMPro;
using UnityEngine;

namespace UI.Buttons
{
    public class CountEnemyPanel : MonoCache
    {
        [SerializeField] private TMP_Text _text;
        private SaveLoadService _saveLoadService;
        private int _countEnemy;
        public void Initialize(SaveLoadService saveLoadService)
        {
            _saveLoadService= saveLoadService;
            saveLoadService.OnEnemyDeath += OnEnemyDeath;
            SetCount(_saveLoadService.MaxEnemiesOnScene);
        }

        private void SetCount(int count)
        {
            _countEnemy = count;
            _text.text=_countEnemy.ToString();
        }
        
        private void OnEnemyDeath(Entity enemy)
        {
            _countEnemy--;
            SetCount(_countEnemy);
        }
        
    }
}