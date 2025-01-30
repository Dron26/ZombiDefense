using Data;
using Interface;

namespace Services.SaveLoad
{
    public class AudioSettingsHandler:IAudioSettingsHandler
    {
        private readonly AudioData _audioData;

        public AudioSettingsHandler(AudioData audioData)
        {
            _audioData=audioData;
        }

        public AudioData GetAudioData()
        {
            return _audioData;
        }

        public void Reset()
        {
        }
    }
}