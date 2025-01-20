using System;

namespace Data
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