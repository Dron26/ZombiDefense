using UnityEngine;

namespace Services.Audio
{
    public interface IAudioManager : IService
    {
        public bool GetSoundEnabled();
        public bool GetMusicEnabled();
    
        public void SetMenuEnabled(bool value);
    
        public void ToggleSound(bool value);

        public void ToggleMusic(bool value);

        public void SetMusicVolume(float volume);

        public void SetSoundVolume(float volume);

        public AudioSource GetSoundSource();

        public AudioSource GetMusicSource();

        public void Initialize();
    }
}