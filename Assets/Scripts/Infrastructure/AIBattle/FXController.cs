using System;
using Audio;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.WeaponManagment;
using Observer;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    public class FXController : MonoCache,IObserverByHumanoid,IObserverByWeaponController
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
        private AudioController _audioController;
        private WeaponController _weaponController;
        private void Awake()
        {
            if (TryGetComponent(out Humanoid humanoid))
            {
                humanoid.AddObserver(this);
            }
            else if (TryGetComponent(out Enemy enemy))
            {
                enemy.AddObserver(this);
            }
        }

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

        public void SetAudioSource(AudioSource audioSource)
        {
            _audioSource=audioSource;
        }

        public void NotifyFromHumanoid(object data)
        {
            Humanoid humanoid = GetComponent<Humanoid>();
            _audioController=humanoid.GetAudioController();
            _audioSource= _audioController.GetSoundSource();
            
            _weaponController=humanoid.GetComponent<WeaponController>();
            _weaponController.AddObserver(this);
        }
        
        private void OnDisable()
        {
            if (TryGetComponent(out Humanoid humanoid))
            {
                humanoid.RemoveObserver(this);
                _weaponController.RemoveObserver(this);
            }
            else if (TryGetComponent(out Enemy enemy))
            {
                enemy.RemoveObserver(this);
            }
            
            
        }

        public void NotifyFromWeaponController(Weapon weapon)
        {
            _weapon = weapon;
            _shoot=_weapon.Shoot;
            _reload=_weapon.Reload;
        }

        public void NotifyFromWeaponController(object data)
        {
            throw new NotImplementedException();
        }
    }
}