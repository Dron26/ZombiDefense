using System;

namespace Service.Audio
{
    [Serializable]
    public class AudioData
    {
        public bool SoundEnabled=true;
        public bool MusicEnabled=true;
        public float CurrentVolumeSound = 1f;
        public float CurrentVolumeMusic = 1f;
    }
}