using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WaveManagment;
using UnityEngine;

namespace UI.Levels
{
    public class Location : MonoCache
    {
        public bool IsCompleted => _isCompleted;

        [SerializeField] private List<Wave> _waves;
        [SerializeField] private int _maxEnemyOnLevel;
        
        private bool _isCompleted;
        public int MaxEnemyOnLevel => _maxEnemyOnLevel;

        public List<Wave> GetWaveDataInfo()
        {
            return new List<Wave>(_waves);
        }
        public void SetCompletedLevel(bool isCompleted)
        {
            _isCompleted = isCompleted;
        }

        public void ChangeMaxEnemyOnLevel(int number)
        {
            _maxEnemyOnLevel = number;
        }
    }
}