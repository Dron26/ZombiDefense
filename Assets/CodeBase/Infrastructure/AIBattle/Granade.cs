using System.Collections;
using Infrastructure.AIBattle.AdditionalEquipment;
using Service.Audio;
using UnityEngine;
using Infrastructure.Logic.WeaponManagment;

namespace Infrastructure.AIBattle
{
    [RequireComponent(typeof(ExplosionManager))]
    [RequireComponent(typeof(GranadeAudioPlayer))]
    public class Granade : Weapon
    {
        private ParticleSystem _explosionEffect;
        private AudioClip _explosionSound;
        private ExplosionManager _explosionManager;
        private GranadeAudioPlayer _granadeAudio;
        public WeaponType Type => WeaponType.Grenade;
        private float _sourceVolume;
        private float _timeBeforeExplosion;

        private void Start()
        {
            _timeBeforeExplosion = TimeBeforeExplosion;
            _granadeAudio = GetComponent<GranadeAudioPlayer>();
            _explosionManager = GetComponent<ExplosionManager>();
        }

        private IEnumerator StartCountdown()
        {
            while (_timeBeforeExplosion > 0)
            {
                _timeBeforeExplosion -= Time.deltaTime;
                yield return null;
            }

            Explode();
        }

        public void Throw(float volume)
        {
            _sourceVolume = volume;
            StartCoroutine(StartCountdown());
        }

        private void Explode()
        {
            _explosionManager.ExecuteExplosion(transform.position, Range, Damage, _explosionEffect, _sourceVolume);
            _granadeAudio.PlaySound(_explosionSound, transform.position, _sourceVolume);
            Destroy(gameObject);
        }

        public override void Initialize(ItemData itemData)
        {
            _explosionSound = itemData.ActionClip;
            _explosionEffect = itemData.ExplosionEffect;
        }
    }
}