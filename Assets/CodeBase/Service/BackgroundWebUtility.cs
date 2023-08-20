using System;
using Agava.WebUtility;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using UnityEngine;

namespace Service
{
    public class BackgroundWebUtility:MonoCache,IPauseHandler
    {
        public static BackgroundWebUtility Instance { get; private set; }
        public PauseService PauseService { get; private set; }
        private bool _isPlayAds;
        private void Awake() =>
            Instance = this;
        
        private void Start()
        {
            Initialize();
            PauseService.Register(this);
        }
        
        private void Initialize()
        {
            try
            {
                PauseService = new PauseService();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to initialize PauseService: {e}");
            }
        }
        
        protected override void OnEnabled()
        {
            //WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
        }

        protected override void OnDisabled()
        {
          //  WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
        }

        public void SetPaused(bool isPaused)
        {
            if (_isPlayAds)
                return;

            Time.timeScale = isPaused ? 0.0f : 1.0f;
            AudioListener.pause = isPaused;
            AudioListener.volume = isPaused ? 0f : 1f;
        }
        
        public void SetPauseWhenAds(bool isPaused)
        {
            SetPaused(isPaused);
            _isPlayAds = true;
        }

        public void ClosePauseAds(bool isPaused)
        {
            _isPlayAds = false;
            SetPaused(isPaused);
        }

        private void OnInBackgroundChange(bool inBackground)
        {
            if (PauseService == null)
                return;

            PauseService.SetPaused(inBackground);
        }

       
        
        private void OnApplicationFocus(bool hasFocus)
        {
 //           OnInBackgroundChange(!hasFocus);
        }
    }
}