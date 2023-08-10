using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace UI.Unit
{
    [DisallowMultipleComponent]
    
    public class TempUIUnit : MonoCache
    {
        private ParticleSystem _glow;

        public void Initialize() => _glow=GetComponentInChildren<ParticleSystem>();

        public void Play() => _glow.Play();
    }
}
