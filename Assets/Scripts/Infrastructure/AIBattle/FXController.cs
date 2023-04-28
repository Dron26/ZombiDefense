using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    public class FXController : MonoCache
    {
        [SerializeField] private ParticleSystem _particleAttack;
        [SerializeField] private ParticleSystem _particleHit;
        [SerializeField] private ParticleSystem _particleDie;

        [SerializeField] private AudioSource _audioAttack;
        [SerializeField] private AudioSource _audioDie;

        protected override void OnEnabled()
        {
            _particleAttack.Stop();
            _particleHit.Stop();
            _particleDie.Stop();
            
            _audioAttack.Stop();
            _audioDie.Stop();
        }

        public void OnAttackFX()
        {
            _particleAttack.Play();
            _audioAttack.Play();
        }
        
        public void OnHitFX() => 
            _particleHit.Play();

        public void OnDieFX()
        {
            _particleDie.Play();
            _audioDie.Play();
        }
    }
}