using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    public  class GranadeAudioPlayer:MonoCache
    {
        public void PlaySound(AudioClip clip, Vector3 position, float volume)
        {
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }
    }
}