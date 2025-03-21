using System.Collections.Generic;
using UnityEngine;

namespace Services.PauseService
{
    public interface IPauseService:IService
    {
        void ChangePause(bool isPaused);
        void Resume();
        bool IsPaused { get; }
    }
}


namespace Services.PauseService
{
    public class PauseService : IPauseService
    {
        private bool _isPaused;
        public bool IsPaused => _isPaused;
        private float _timePause = 0;
        private float _timeNormal = 1;

        public void SetPause()
        {
            if (_isPaused) return;
            _isPaused = true;
            _timeNormal = Time.timeScale;
            Time.timeScale = _timePause;
        }

        public void Resume()
        {
            if (!_isPaused) return;
            _isPaused = false;
            Time.timeScale = _timeNormal;
        }

        public void TogglePause()
        {
            if (_isPaused)
            {
                Resume();
            }
            else
            {
                SetPause();
            }
        }

        public void ChangePause(bool isPaused)
        {
            if (isPaused)
            {
                SetPause();
               
            }
            else
            {
                Resume();
                
            }
        }
    }
}