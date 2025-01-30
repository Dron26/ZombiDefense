using System.Collections;
using System.Collections.Generic;
using Enemies;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using Services.Audio;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    public class EnemyFXController : MonoCache
    {
        [SerializeField] private List<ParticleSystem> _particlesHitLite;
        [SerializeField] private List<ParticleSystem> _particlesHitSniperRifle;
        [SerializeField] private List<ParticleSystem> _particlesFire;
        [SerializeField] private List<List<ParticleSystem>> _particlesGroup = new();
        [SerializeField] private ParticleSystem _particleExplosion;
        [SerializeField] private ParticleSystem _particleTankDie;
        [SerializeField] private ParticleSystem _particleTankDie1;
        [SerializeField] private List<ItemType> _weaponNames;
        [SerializeField] private ParticleSystem _bloodFlowing;
        [SerializeField] private Dictionary<ItemType, List<ParticleSystem>> _particleByType = new();
        [SerializeField] private float _areaWidth = 0.1f;
        [SerializeField] private float _areaHeight = 0.1f;
        [SerializeField] private float _minParticleScale = 1f;
        [SerializeField] private float _maxParticleScale = 1f;
        [SerializeField] private Transform _container;
        [SerializeField] private ParticleSystem _impactDirt;
        [SerializeField] private GameObject _shield;
        
        private ItemType _item;
        private AudioManager _audioManager;
        private bool _isFireParticlePlay;
        private EnemyType _enemyType;
        private AudioSource _audioSource;
        private Enemy _enemy;
        private WaitForSeconds _waitThrower;
        private WaitForSeconds _waitFire;
        private void Awake()
        {
            if (TryGetComponent(out Enemy enemy))
            {
                _enemy = enemy;
                _enemy.OnEnemyEvent += HandleEnemyEvent;
                _enemy.OnInitialized += SetData;
                //enemy.OnTakeDamage += OnHitFX;
                // enemy.OnDeath += OnDieFX;
                _waitThrower = new WaitForSeconds(0.5f);
                _waitFire = new WaitForSeconds(3f);
            }
        }

        private void HandleEnemyEvent(EnemyEventType eventType, ItemType itemType)
        {
            _item = itemType;

            switch (eventType)
            {
                case EnemyEventType.Death:
                    OnDieFX();
                    break;
                case EnemyEventType.TakeDamage:
                    OnTakeDamage();
                    break;
                case EnemyEventType.TakeSmokerDamage:
                    break;
                case EnemyEventType.TakeSimpleWalkerDamage:
                    break;
            }
        }

        private void OnHitFire()
        {
            if (!_isFireParticlePlay)
            {
                StartCoroutine(FireAttackTimer());
            }
        }

        private IEnumerator FireAttackTimer()
        {
            foreach (var particle in _particlesFire)
            {
                particle.gameObject.SetActive(true);
                particle.Play();
                _isFireParticlePlay = true;
            }

            yield return _waitFire;

            foreach (var particle in _particlesFire)
            {
                particle.Stop();
                particle.gameObject.SetActive(false);
            }

            _isFireParticlePlay = false;
        }

        private void SetData()
        {
            _audioManager = _enemy.GetAudioController();
            _audioSource = _audioManager.GetSoundSource();
            _enemyType = _enemy.Data.Type;
        }
        public IEnumerator OnThrowFlesh(Vector3 position)
        {
            ParticleSystem particleThrow = new ParticleSystem();
            
            particleThrow=Instantiate(_enemy.Data.ThrowAbility.ThrowerComponent, _container.position, _container.transform.rotation,_container);

            particleThrow.Play();
            
            yield return _waitThrower;
            
            Instantiate(_enemy.Data.ThrowAbility.ToxicBoiling, position+_enemy.Data.ThrowAbility.ToxicBoiling.transform.position,  _enemy.Data.ThrowAbility.ToxicBoiling.transform.rotation);

            yield break;
        }
        
        
        public void OnTankDeathFX()
        {
            ParticleSystem particleTankDie = Instantiate(_particleTankDie, _container.position, Quaternion.identity);
            ParticleSystem particleTankDie1 = Instantiate(_particleTankDie1, _container.position, Quaternion.identity);

            particleTankDie.Play();
            particleTankDie1.Play();
        }

        public void OnTakeDamage()
        {
            if (_item == ItemType.Flammer)
            {
                OnHitFire();
            }
            else
            {
                PlayRandomParticleEffect();
            }
        }

        public void OnDieFX()
        {
            if (_enemyType == EnemyType.Smoker)
            {
                SmokerExplosion();
            }
        }

        private void SmokerExplosion()
        {
            ParticleSystem particleExplosion =
                Instantiate(_particleExplosion, _container.position, Quaternion.identity);
            particleExplosion.Play();
            _audioSource.PlayOneShot(_enemy.Data.ExplosiveAbility.ExplosionClip);
        }

        public void PlayRandomParticleEffect()
        {
            int randomIndex = Random.Range(0, _particlesHitLite.Count);
            ParticleSystem particle = Instantiate(_particlesHitLite[randomIndex], _container.position, Quaternion.identity);
                
            particle.Play();
        }

        private void SetRandomParticlePosition(ParticleSystem particleSystem)
        {
            // Задаем случайные координаты внутри области
            float randomY = Random.Range(-_areaHeight / 2f, _areaHeight / 2f);
            Vector3 randomPosition = new Vector3(0f, randomY, 0f);
            particleSystem.transform.position = transform.position + randomPosition;
        }

        private void SetRandomParticleScale(ParticleSystem particleSystem)
        {
            // Меняем размер частицы в небольших диапазонах
            float randomScale = Random.Range(_minParticleScale, _maxParticleScale);
            particleSystem.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        }

        protected override void OnDisable()
        {
            if (TryGetComponent(out Enemy enemy))
            {
                enemy.OnInitialized -= SetData;
            }
        }


        public void ShieldDamage()
        {
            ParticleSystem particle = Instantiate(_impactDirt);
            SetRandomParticlePosition(particle);
            particle.Play();
        }
    }
}