using Agava.WebUtility;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Data.Settings.Audio
{
    public class WebSoundMuter : MonoCache
    {
        protected override void OnEnabled()
        {
            WebApplication.InBackgroundChangeEvent += BackgroundSound;
        }

        protected override void OnDisabled()
        {
            WebApplication.InBackgroundChangeEvent -= BackgroundSound;
        }

        private void BackgroundSound(bool inBackground)
        {
            AudioListener.pause = inBackground;
            AudioListener.volume = inBackground ? 0f : 1f;
        }
    }
}