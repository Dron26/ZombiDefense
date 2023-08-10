using System;
using System.Collections.Generic;
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
        private AudioManager _audioManager;
        private WeaponController _weaponController;
        private Humanoid _humanoid;
        private void Awake()
        {
             _humanoid = GetComponent<Humanoid>();
             _humanoid.AddObserver(this);
             _weaponController= GetComponent<WeaponController>();
             _weaponController.AddObserver(this);
            
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
            _audioManager=_humanoid.GetAudioController();
            _audioSource= _audioManager.GetSoundSource();
        }

        public void NotifySelection(bool isSelected)
        {
            throw new NotImplementedException();
        }

        public void NotifyFromWeaponController(Weapon weapon)
        {
            _weapon = weapon;
            _shoot=_weapon.Shoot;
            _reload=_weapon.Reload;
        }
        
        protected override void  OnDisable()
        {
            _humanoid.RemoveObserver(this);
            _weaponController.RemoveObserver(this);
        }
    }
}