using Data;
using Services;

namespace Interface
{
    public interface IAudioSettingsHandler:IService
    {
        AudioData GetAudioData();
        void Reset();
    }
}