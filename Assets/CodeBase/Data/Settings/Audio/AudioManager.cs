using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Audio
{
    public class AudioManager : MonoCache
    {
        private SaveLoadService _saveLoadService;
        private Audio _audioParametrs;
        
        private AudioMixer _mixerSound;
        private AudioMixer _mixerMusic;
        public static AudioManager instance; 
        public float _maxVolume = 100.0f;  
        public float _minVolume = -80.0f;  
        
        private float _currentVolumeSound;
        private float _currentVolumeMusic;
        
        public  bool SoundEnabled=>_soundEnabled;
        public bool MusicEnabled=>_musicEnabled; 
        private bool _soundEnabled;  
        private bool _musicEnabled;  
        
        private AudioClip _previousBackgroundMusic;
        private AudioClip _backgroundMusic;
        private Coroutine _musicCoroutine;
        
        [SerializeField] private List< AudioClip> _backgroundMusics; 
        [SerializeField] private List< AudioClip> _currentMusics; 
        [SerializeField] private List< AudioClip> _backgroundGeneralMenuMusics; 
      //  [SerializeField] private List< AudioClip> _soundButtons; 
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundSource;
        
        private bool isGeneralMenuEnabled = false;
        public void Initialize(SaveLoadService saveLoadService)
        {
            _saveLoadService=saveLoadService;
            _mixerSound = (AudioMixer)Resources.Load("MixerSound");
            _mixerMusic = (AudioMixer)Resources.Load("MixerMusic");
            
            CheckMixerException();

            _audioParametrs=_saveLoadService.GetAudioSettings();
            _soundEnabled = _audioParametrs.SoundEnabled;
            _musicEnabled = _audioParametrs.MusicEnabled;
            _currentVolumeSound = _audioParametrs.CurrentVolumeSound;
            _currentVolumeMusic = _audioParametrs.CurrentVolumeMusic;
       //     _soundSource.clip = _soundButtons[0];
            SetBackgroundMusics();
            
            SetMusicVolume(_currentVolumeMusic);
            SetSFXVolume(_currentVolumeSound);
            
            SetParameters();;
        }
        
        public void SetGeneralMenuEnabled(bool value)
        {
            isGeneralMenuEnabled = value;
        }

        private void SetBackgroundMusics()
        {
            if (isGeneralMenuEnabled)
            {
                _backgroundMusic = _backgroundGeneralMenuMusics.FirstOrDefault();;
            }

            else
            {
                _backgroundMusic=_backgroundMusics[Random.Range(0,_backgroundMusics.Count)];
            }
               

               
            _musicSource.clip = _backgroundMusic;
            _musicSource.loop = true;
            print(_musicSource.clip.name);
        }

        private void Start()
        {
            // if (_musicEnabled!=false&&isGeneralMenuEnabled==false) StartCoroutine(MusicCoroutine());
        }

        private void CheckMixerException()
        {
            if (_mixerSound == null)
            {
                Debug.LogError("MixerSound not found!");
            }
            if (_mixerMusic == null)
            {
                Debug.LogError("MixerMusic not found!");
            }
        }

        private  void SetParameters()
        {
            if (_soundEnabled==false)
            {
                ToggleSound(false);
            }

            if (_musicEnabled == true)
            {
                PlayBackgroundMusic();
            }
            else
            {
                StopBackgroundMusic();
            }
        }

        public void ToggleSound(bool value)
        {
            _soundEnabled = value;
            
            _soundSource.mute = !value;
        }

        public void ToggleMusic(bool value)
        {
            _musicEnabled = value;

            if (_musicEnabled==false)
            {
                StopBackgroundMusic();
                
                if (isGeneralMenuEnabled==false)
                {
                    if (_musicCoroutine != null)
                    {
                        StopCoroutine(_musicCoroutine);
                        _musicCoroutine = null;
                    }
                }
                
            }
            else
            {
                PlayBackgroundMusic();
                // if (isGeneralMenuEnabled == false)
                // {
                //     if (_musicCoroutine == null)
                //     {
                //         _musicCoroutine = StartCoroutine(MusicCoroutine());
                //     }
                // }
            }
        }
        
        public void SetMusicVolume(float volume)
        {
            _currentVolumeMusic = Mathf.Clamp(volume, _minVolume, _maxVolume);
            _musicSource.volume=_currentVolumeMusic;
        }

        public void SetSFXVolume(float volume)
        {
            _currentVolumeSound = Mathf.Clamp(volume, _minVolume, _maxVolume);
            _soundSource.volume=_currentVolumeSound;
        }
        
        public void PlayBackgroundMusic()
        {
            if (_musicSource.clip != null && !_musicSource.isPlaying)
            {
                if (_musicEnabled==true)
                {
                    _musicSource.Play();
                }
            }
        }

        
        public void StopBackgroundMusic()
        {
            if (_musicSource.isPlaying)
            {
                _musicSource.Stop();
            }
        }
        

        private Audio GetAudioSettings()
        {
            Audio parametrs = new Audio();

            parametrs.SoundEnabled = _soundEnabled;
            parametrs.MusicEnabled = _musicEnabled;
            parametrs.CurrentVolumeMusic = _currentVolumeMusic;
            parametrs.CurrentVolumeSound = _currentVolumeSound;
            
            return parametrs;
        }

        protected override void OnDisable()
        {
            UpdateParameter();
        }

        public void UpdateParameter()
        {
            _saveLoadService.SetAudioSettings(GetAudioSettings());
        }
        
        private void PlayRandomMusic()
        {
            if (!isGeneralMenuEnabled)
            {
                AudioClip newClip = _backgroundMusics[Random.Range(0, _backgroundMusics.Count)];
            
                while (newClip == _previousBackgroundMusic)
                {
                    newClip = _backgroundMusics[Random.Range(0, _backgroundMusics.Count)];
                }
            
                _previousBackgroundMusic = newClip;
            
                _musicSource.clip = newClip;
                _musicSource.loop = true;
                _musicSource.Play();
            }
        }

        private IEnumerator MusicCoroutine()
        {
            while (_musicEnabled)
            {
                PlayRandomMusic();
                yield return new WaitForSeconds(Random.Range(30, 120));
            }
        }

        public AudioSource GetSoundSource()
        {
            return _soundSource;
        }
        
        public AudioSource GetMusicSource()
        {
            return _musicSource;
        }
        
    }

    [Serializable]
    public class Audio
    {
        public bool SoundEnabled=true;
        public bool MusicEnabled=true;
        public float CurrentVolumeSound = 1f;
        public float CurrentVolumeMusic = 1f;
    }
}