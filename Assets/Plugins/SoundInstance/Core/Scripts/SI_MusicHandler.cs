﻿using System.Collections;
using UnityEngine;

namespace Plugins.SoundInstance.Core.Scripts
{
    public class SI_MusicHandler : MonoBehaviour
    {
        private bool _isAllowed;
        private float _fade = 1.5f;
        private MusicsStore _storage;
        private bool _needStop;
        private bool _isActiveMenu;
        private bool _isActiveGameplay;
            private bool _isActiveLoading;
        public AudioSource audioSource
        {
            get { return GetComponent<AudioSource>(); }
        }

        private void Awake()
        {
            foreach (GameObject go in FindObjectsOfType<GameObject>())
            {
                if (go.name == "[SoundInstanceMusicLive]")
                {
                    Destroy(gameObject);
                    return;
                }
            }

            Setup();
        }

        private void Update()
        {
            if (_isAllowed && !audioSource.isPlaying)
            {
                if (!_needStop)
                    StartGameplayMusic(GetRandomMusic().name, _fade);
                else
                    StopMusic(_fade, true);
            }
        }

        public void StopRandomMusic(bool fading)
        {
            _isAllowed = false;
            StopMusic(_fade, fading);
        }

        private void Setup()
        {
            _storage = (MusicsStore)UnityEngine.Resources.Load("SIMusicStore");
            gameObject.name = "[SoundInstanceMusicLive]";
            Static.SoundInstance.SetMusicHandler(this);
            DontDestroyOnLoad(this);
            _isAllowed = true;
            _needStop = false;
        }

        public void SetStartFade(float fade) =>
            _fade = fade;

        public void StartGameplayMusic(string name, float fadeSpeed)
        {
            _isActiveMenu = false;
            _isActiveGameplay = true;
            _isActiveLoading= false;
            
            if (audioSource.isPlaying)
            {
                StopAllCoroutines();
                StartCoroutine(Switch(name, fadeSpeed));
                return;
            }

            AudioClip audioClip = _storage.GetGameplayMusic(name).Song;
            audioSource.clip = audioClip;
            StartCoroutine(Play(2 / fadeSpeed));

            foreach (GameObject go in FindObjectsOfType<GameObject>())
            {
                Static.SoundInstance.CurrentMusic = _storage.GetGameplayMusic(name);
                go.SendMessage("OnMusicStarted", _storage.GetGameplayMusic(name), SendMessageOptions.DontRequireReceiver);
            }
        }
        
        public void StartMenuMusic(string name, float fadeSpeed)
        {
            _isActiveMenu = true;
            _isActiveGameplay = false;
            _isActiveLoading= false;
            
            if (audioSource.isPlaying)
            {
                StopAllCoroutines();
                StartCoroutine(Switch(name, fadeSpeed));
                return;
            }

            AudioClip audioClip = _storage.GetMenuMusic(name).Song;
            audioSource.clip = audioClip;
            StartCoroutine(Play(2 / fadeSpeed));

            // ... (дополнительная логика, если необходимо)
        }

        public void StartLoadingMusic(string name, float fadeSpeed)
        {
            _isActiveMenu = false;
            _isActiveGameplay = false;
            _isActiveLoading= true;
            
            if (audioSource.isPlaying)
            {
                StopAllCoroutines();
                StartCoroutine(Switch(name, fadeSpeed));
                return;
            }

            AudioClip audioClip = _storage.GetLoadingMusic(name).Song;
            audioSource.clip = audioClip;
            StartCoroutine(Play(2 / fadeSpeed));

            // ... (дополнительная логика, если необходимо)
        }

        public void StopMusic(float fadeSpeed, bool fading)
        {
            StopAllCoroutines();
            StartCoroutine(Stop(fadeSpeed, fading));
        }

        public void PauseMusic(float fadeSpeed)
        {
            StopAllCoroutines();
            StartCoroutine(Pause(fadeSpeed));
        }

        public void ResumeMusic(float fadeSpeed)
        {
            StopAllCoroutines();
            StartCoroutine(Resume(fadeSpeed));
        }

        private IEnumerator Play(float duration)
        {
            float currentTime = 0;
            float start = 0;

            audioSource.volume = 0;
            audioSource.Play();

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start,
                    Static.SoundInstance.musicVolume, currentTime / duration);
                yield return null;
            }

            yield break;
        }

        private IEnumerator Switch(string name, float fadeSpeed)
        {
            float duration = 1 / fadeSpeed;
            float currentTime = 0;
            float start = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, 0, currentTime / duration);
                yield return null;
            }

            audioSource.Stop();

            if (_isActiveGameplay)
            {
                AudioClip audioClip = _storage.GetGameplayMusic(name).Song;
                audioSource.clip = audioClip;

                Static.SoundInstance.CurrentMusic = _storage.GetGameplayMusic(name);
            }
            else if (_isActiveMenu)
            {
                AudioClip audioClip = _storage.GetMenuMusic(name).Song;
                audioSource.clip = audioClip;
                
                Static.SoundInstance.CurrentMusic = _storage.GetMenuMusic(name);
            }
            else if (_isActiveLoading)
            {
                AudioClip audioClip = _storage.GetLoadingMusic(name).Song;
                audioSource.clip = audioClip;
                
                Static.SoundInstance.CurrentMusic = _storage.GetLoadingMusic(name);
            }



            StartCoroutine(Play(1 / fadeSpeed));
            yield break;
        }

        private IEnumerator Stop(float fadeSpeed, bool fading)
        {
            if (fading)
            {
                float duration = 1 / fadeSpeed;
                float currentTime = 0;
                float start = audioSource.volume;

                while (currentTime < duration)
                {
                    currentTime += Time.deltaTime;
                    audioSource.volume = Mathf.Lerp(start, 0, currentTime / duration);
                    yield return null;
                }
            }

            audioSource.Stop();
            _needStop = false;

            foreach (GameObject go in FindObjectsOfType<GameObject>())
            {
                go.SendMessage("OnMusicStopped", SendMessageOptions.DontRequireReceiver);
            }

            yield break;
        }

        private IEnumerator Pause(float fadeSpeed)
        {
            float duration = 1 / fadeSpeed;
            float currentTime = 0;
            float start = audioSource.volume;
            
                while (currentTime < duration&&Time.deltaTime!=0)
                {
                    currentTime += Time.deltaTime;
                    audioSource.volume = Mathf.Lerp(start, 0, currentTime / duration);
                    yield return null;
                }
            
                _needStop = true;
            audioSource.Pause();

            // foreach (GameObject go in FindObjectsOfType<GameObject>())
            // {
            //     go.SendMessage("OnMusicPaused", SendMessageOptions.DontRequireReceiver);
            // }

            yield break;
        }

        private IEnumerator Resume(float fadeSpeed)
        {
            float duration = 1 / fadeSpeed;
            float currentTime = 0;
            float start = audioSource.volume;

            // foreach (GameObject go in FindObjectsOfType<GameObject>())
            // {
            //     go.SendMessage("OnMusicResume", SendMessageOptions.DontRequireReceiver);
            // }

            audioSource.UnPause();
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start,
                    Static.SoundInstance.musicVolume, currentTime / duration);
                yield return null;
            }

            yield break;
        }

        public Music GetNextMusic()
        {
            if (Static.SoundInstance.CurrentMusic == null)
            {
                return _storage.GetMusicByIndex(0);
            }

            if (!(_storage.GetMusicByIndex(
                      _storage.GetMusicIndex(Static.SoundInstance.CurrentMusic) +
                      1) ==
                  null))
            {
                return _storage.GetMusicByIndex(
                    _storage.GetMusicIndex(Static.SoundInstance.CurrentMusic) + 1);
            }
            else
            {
                return _storage.GetMusicByIndex(0);
            }
        }

        private Music GetRandomMusic()
        {
            _needStop = false;
            return _storage.GetRandomMusic();
        }

        public void StartRandomMusic()
        {
            _isActiveMenu = false;
            _isActiveGameplay = true;
            _isActiveLoading= false;
            _isAllowed = true;
        }
    }
}