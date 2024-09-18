using System.Collections.Generic;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using Service.Audio;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    public class EnemyFXController : MonoCache
    {
        [SerializeField] private List<ParticleSystem> _particlesHit;
        [SerializeField] private List<ParticleSystem> _particlesHitLite;
        [SerializeField] private List<ParticleSystem> _particlesHitSniperRifle;
        [SerializeField] private List<List<ParticleSystem>> _particlesGroup = new();
        [SerializeField] private ParticleSystem _particleTankDie;
        [SerializeField] private ParticleSystem _particleTankDie1;
        [SerializeField] private List<ItemType> _weaponNames;
        [SerializeField] private ParticleSystem _bloodFlowing;
        [SerializeField] private Dictionary<ItemType, List<ParticleSystem>> _particleByType = new();
        [SerializeField] private float _areaWidth = 0.1f;
        [SerializeField] private float _areaHeight = 0.1f;
        [SerializeField] private float _minParticleScale = 1f;
        [SerializeField] private float _maxParticleScale = 1f;
        
        private ItemType _item;
        private AudioManager _audioManager;

        private void Awake()
        {
            if (TryGetComponent(out Enemy enemy))
            {
                enemy.OnEnemyEvent += HandleEnemyEvent;
                enemy.OnInitialized += SetAudio; 
                //enemy.OnTakeDamage += OnHitFX;
               // enemy.OnDeath += OnDieFX;
                
            }
        }
        
        private void HandleEnemyEvent(EnemyEventType eventType,ItemType itemType)
        {
            _item=itemType;
            
            switch (eventType)
            {
                case EnemyEventType.Death:
                    OnDieFX();
                    break;
                case EnemyEventType.TakeDamage:
                    break;
                case EnemyEventType.TakeSmokerDamage:
                    break;
                case EnemyEventType.TakeSimpleWalkerDamage:
                    OnSimpleWalkerDamage();
                    break;
            }
        }

        private void SetAudio(Enemy enemy)
        {
            _audioManager = enemy.GetAudioController();
        }

        public void OnTankDeathFX()
        {
            ParticleSystem particleTankDie = Instantiate(_particleTankDie, transform.position, Quaternion.identity);
            ParticleSystem particleTankDie1 = Instantiate(_particleTankDie1, transform.position, Quaternion.identity);

            particleTankDie.Play();
            particleTankDie1.Play();
        }

        public void OnSimpleWalkerDamage()
        {
            PlayRandomParticleEffect();
        }

        public void OnDieFX()
        {
            if (_bloodFlowing)
            {
                _bloodFlowing.Stop();
            }
            else
            {
                _bloodFlowing.Play();
            }
        }
        
        public void PlayRandomParticleEffect()
        {
            
            // if (_particleByType.TryGetValue(_weapon, out List<ParticleSystem> effect))
            // {
            //     if (effect.Count > 0)
            //     {
            //         int randomIndex = UnityEngine.Random.Range(0, effect.Count);
            //         ParticleSystem particleSystem = effect[randomIndex];
            //
            //         //SetRandomParticlePosition(particleSystem);
            //         //SetRandomParticleScale(particleSystem);
            //
            //         particleSystem.Play();
            //     }
            //     else
            //     {
            //         Debug.LogWarning($"No particle effects found for weapon name '{_weapon}'.");
            //     }
            // }
            // else
            // {
            //     Debug.LogWarning($"No particle effects found for weapon name '{_weapon}'.");
            // }

            if (_item!=ItemType.SniperRifle)
            {
                int randomIndex = Random.Range(0, _particlesHitLite.Count);
                _particlesHitLite[randomIndex].Play();
            }
            else
            {
                int randomIndex = Random.Range(0, _particlesHitSniperRifle.Count);
                _particlesHitSniperRifle[randomIndex].Play();
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

            float randomNumber = UnityEngine.Random.Range(0, maxChance);

            if (randomNumber == chance)
            {
                _bloodFlowing.Play();
            }
        }

        protected override void OnDisable()
        {
            if (TryGetComponent(out Enemy enemy))
            {
                enemy.OnInitialized -= SetAudio;
            }
        }
    }
}