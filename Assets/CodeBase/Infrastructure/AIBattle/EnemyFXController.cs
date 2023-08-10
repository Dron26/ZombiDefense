using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.WeaponManagment;
using Observer;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    public class EnemyFXController : MonoCache,IObserverByHumanoid,IObserverByWeaponController
    {
       
        [SerializeField] private List<ParticleSystem> _particlesHit;
        
        [SerializeField] private List<ParticleSystem> _particlesHitLite;
        [SerializeField] private List<ParticleSystem> _particlesHitSniperRifle;
        
        [SerializeField] private  List< List<ParticleSystem> >  _particlesGroup=new();
        
        [SerializeField] private ParticleSystem _particleTankDie;
        [SerializeField] private ParticleSystem _particleTankDie1;
        [SerializeField] private List<WeaponType> _weaponNames;
        [SerializeField] private ParticleSystem _bloodFlowing ;
        [SerializeField] private Dictionary<WeaponType,List<ParticleSystem> > _particleByType=new();
        
        [SerializeField] private float _areaWidth = 0.1f;
        [SerializeField] private float _areaHeight = 0.1f;
        [SerializeField] private float _minParticleScale = 1f;
        [SerializeField] private float _maxParticleScale = 1f;
        
        
         private AudioClip _shoot;
         private AudioClip _reload;
        private AudioSource _audioSource;
        private Weapon _weapon;
        private AudioManager _audioManager;
        private WeaponController _weaponController;

        private void Awake()
        {
            if (TryGetComponent(out Enemy enemy))
            {
                enemy.AddObserver(this);
            }

            _particlesGroup.Add(_particlesHitLite);
            _particlesGroup.Add(_particlesHitSniperRifle);
            
            for (int i = 0; i < _weaponNames.Count; i++)
            {
                _particleByType.Add(_weaponNames[i],_particlesGroup[i]);
            }

        }

        public void OnAttackFX()
        {
        }
        
        public void OnTankDeathFX()
        {
            ParticleSystem particleTankDie = Instantiate(_particleTankDie, transform.position, Quaternion.identity);
            ParticleSystem particleTankDie1 = Instantiate(_particleTankDie1, transform.position, Quaternion.identity);
            
            particleTankDie.Play();
            particleTankDie1.Play();
        }

        public void OnHitFX(WeaponType weaponWeaponType)
        {
            PlayRandomParticleEffect(weaponWeaponType);
        }

        public void OnDieFX()
        {
            _bloodFlowing.Play();
        }

        public void SetAudioSource(AudioSource audioSource)
        {
            _audioSource=audioSource;
        }

        public void NotifyFromHumanoid(object data)
        {
            Humanoid humanoid = GetComponent<Humanoid>();
            _audioManager=humanoid.GetAudioController();
            _audioSource= _audioManager.GetSoundSource();
            
            _weaponController=humanoid.GetComponent<WeaponController>();
            _weaponController.AddObserver(this);
        }

        public void NotifySelection(bool isSelected)
        {
            throw new NotImplementedException();
        }

        protected override void  OnDisable()
        {
            if (TryGetComponent(out Enemy enemy))
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
        
        public void PlayRandomParticleEffect(WeaponType weaponWeaponType)
        {
            if (_particleByType.TryGetValue(weaponWeaponType, out List<ParticleSystem> effect))
            {
                if (effect.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, effect.Count);
                    ParticleSystem particleSystem = effect[randomIndex];

                    //SetRandomParticlePosition(particleSystem);
                        //SetRandomParticleScale(particleSystem);

                    particleSystem.Play();
                }
                else
                {
                    Debug.LogWarning($"No particle effects found for weapon name '{weaponWeaponType}'.");
                }
            }
            else
            {
                Debug.LogWarning($"No particle effects found for weapon name '{weaponWeaponType}'.");
            }
        }

        private void SetRandomParticlePosition(ParticleSystem particleSystem)
        {
            // Задаем случайные координаты внутри области
            float randomX = UnityEngine.Random.Range(-_areaWidth / 2f, _areaWidth / 2f);
            float randomY = UnityEngine.Random.Range(-_areaHeight / 2f, _areaHeight / 2f);
            Vector3 randomPosition = new Vector3(randomX, randomY, 0f);
            particleSystem.transform.position = transform.position + randomPosition;
        }

        private void SetRandomParticleScale(ParticleSystem particleSystem)
        {
            // Меняем размер частицы в небольших диапазонах
            float randomScale = UnityEngine.Random.Range(_minParticleScale, _maxParticleScale);
            particleSystem.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        }

        private void SetFirstHit()
        {
            int maxChance = 20;
            int chance = 14;
            
            float randomNumber = UnityEngine.Random.Range(0,maxChance);
            
            if (randomNumber == chance)
            {
                _bloodFlowing.Play();
            }
        }

    }
}