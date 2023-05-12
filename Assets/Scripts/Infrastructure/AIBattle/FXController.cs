using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.WeaponManagment;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    public class FXController : MonoCache
    {
        [SerializeField] private ParticleSystem _particleHit;
        [SerializeField] private ParticleSystem _particleGunshotSingle;
        [SerializeField] private ParticleSystem _particleEjectSingle;
        [SerializeField] private ParticleSystem _particleDie;
        [SerializeField] private ParticleSystem _particleTankDie;
        [SerializeField] private ParticleSystem _particleTankDie1;
         private AudioClip _shoot;
         private AudioClip _reload;
        private AudioSource _audioSource;
        
        private Weapon _weapon;
        

        public void OnAttackFX()
        {
            _audioSource.PlayOneShot(_shoot);
            _particleGunshotSingle.Play();
        }
        
        public void OnTankDeathFX()
        {
            ParticleSystem particleTankDie = Instantiate(_particleTankDie, transform.position, Quaternion.identity);
            ParticleSystem particleTankDie1 = Instantiate(_particleTankDie1, transform.position, Quaternion.identity);
            
            particleTankDie.Play();
            particleTankDie1.Play();
        }
        
        
        public void OnAttackFXStop()
        {
            _particleGunshotSingle.Stop();
            _particleEjectSingle.Stop();
        }
        
        public void OnHitFX() => 
            _particleHit.Play();

        public void OnDieFX()
        {
            //_particleDie.Play();
           // _audioDie.Play();
        }

        public void OnReloadFX()
        {
            _audioSource.PlayOneShot(_reload);
        }

        public void Initialize(WeaponController weaponController)
        {
            _weapon =weaponController.GetActiveWeapon();
            _shoot=_weapon.Shoot;
            _reload=_weapon.Reload;
            
           // _particleHit=_weapon.
            //_particleEjectSingle.Stop();
            //_audioAttack.Stop();
           // _audioDie.Stop();
        }

        public void SetAudioSource(AudioSource audioSource)
        {
            _audioSource=audioSource;
        }
    }
}