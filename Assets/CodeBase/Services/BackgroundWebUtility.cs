using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services.PauseService;
using UnityEngine;

//using Agava.WebUtility;

namespace Services
{
    public class BackgroundWebUtility:MonoCache,IPauseHandler
    {
        public static BackgroundWebUtility Instance { get; private set; }
        public TimePauseService TimePauseService { get; private set; }
        private bool _isPlayAds;
        private void Awake() =>
            Instance = this;
        
        private void Start()
        {
            Initialize();
            TimePauseService.Register(this);
        }
        
        private void Initialize()
        {
            try
            {
                TimePauseService = new TimePauseService();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to initialize PauseService: {e}");
            }
        }
        
        // protected override void OnEnabled()
        // {
        //     WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
        // }
        //
        // protected override void OnDisabled()
        // {
        //     WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
        // }

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
            if (TimePauseService == null)
                return;

            TimePauseService.SetPaused(inBackground);
        }

       
        
        private void OnApplicationFocus(bool hasFocus)
        {
            OnInBackgroundChange(!hasFocus);
        }
    }
}