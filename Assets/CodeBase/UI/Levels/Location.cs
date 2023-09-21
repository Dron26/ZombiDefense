using System.Collections.Generic;
using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Levels
{
    public class Location : MonoCache
    {
        public int Id => _id;
        public string Path => _path;
        public bool IsTutorial => _isTutorial;
        public bool IsLocked => _isLocked;
        public bool IsCompleted => _isCompleted;

        [SerializeField] private List<WaveData> _waveDatas;
        [SerializeField] private int _id;
        [SerializeField] private string _path;
        [SerializeField] private bool _isTutorial;
        [SerializeField] private bool _isLocked;
        [SerializeField] private Image _locked;
        [SerializeField] private Image _unlocked;
        [SerializeField] private int _maxEnemyOnLevel;
        private bool _isCompleted;
        private Button _button;
        public int MaxEnemyOnLevel => _maxEnemyOnLevel;

        public List<WaveData> GetWaveDataInfo()
        {
            return new List<WaveData>(_waveDatas);
        }

        public void SetLocked(bool isLocked)
        {
            _button=GetComponentInChildren<Button>();
            
            if (isLocked)
            {
                _isLocked = true;
                _locked.enabled=true;
                _unlocked.enabled=false;
                _button.interactable=false;
                _button=GetComponent<Button>();
                _button.image=_locked;
            }
            else
            {
                _isLocked = false;
                _locked.enabled=false;
                _unlocked.enabled=true;
                _button.interactable=true;
                _button=GetComponent<Button>();
                _button.image=_unlocked;
            }
        }

        public void SetCompletedLevel(bool isCompleted)
        {
            _isCompleted = isCompleted;
        }

        public void ChangeMaxEnemyOnLevel(int number)
        {
            _maxEnemyOnLevel = number;
        }

        public void Initialize( int id, string path, bool isTutorial, bool isLocked, int maxEnemyOnLevel)
        {
            _id = id;
            _path = path;
            _isTutorial = isTutorial;
            _isLocked = isLocked;
            _maxEnemyOnLevel = maxEnemyOnLevel;
        }
        
    }
}