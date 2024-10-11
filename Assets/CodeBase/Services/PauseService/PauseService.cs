using System;
using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Services.PauseService
{
    public class PauseService : MonoCache, IPauseService
    {
        private bool _isPaused;
        private List<IPauseListener> _pauseListeners = new List<IPauseListener>();

        public bool IsPaused => _isPaused;

        public event Action OnPaused;
        public event Action OnResumed;
        private float _timePause = 0;
        private float _timeNormal = 1;
        
        public void Subscribe(IPauseListener listener)
        {
            if (!_pauseListeners.Contains(listener))
            {
                _pauseListeners.Add(listener);
            }
        }

        // Отписать объект от событий паузы
        public void Unsubscribe(IPauseListener listener)
        {
            if (_pauseListeners.Contains(listener))
            {
                _pauseListeners.Remove(listener);
            }
        }
        
        
        // Метод для постановки игры на паузу
        public void Pause()
        {
            if (_isPaused) return;
            _isPaused = true;
            foreach (var listener in _pauseListeners)
            {
                listener.OnPaused();
            }
            
                if (_isPaused)
                {
                    _timeNormal=Time.timeScale;
                    
                    if (_timeNormal == 0)
                    {
                        _timeNormal = 1;
                    }
                    
                    Time.timeScale = _timePause;
                }
                else
                {
                    Time.timeScale = _timeNormal;
                }
                
        }

        // Метод для снятия игры с паузы
        public void Resume()
        {
            if (!_isPaused) return;
            _isPaused = false;
            foreach (var listener in _pauseListeners)
            {
                listener.OnResumed();
            }
        }
        
        public void TogglePause()
        {
            if (_isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
}