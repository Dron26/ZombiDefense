using System.Collections;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    [RequireComponent(typeof(ExplosionManager))]
    [RequireComponent(typeof(GranadeAudioPlayer))]
    public class Grenade : Weapon
    {
        private ParticleSystem _explosionEffect;
        private AudioClip _explosionSound;
        private ExplosionManager _explosionManager;
        private GranadeAudioPlayer _granadeAudio;
        public ItemType Type => ItemType.Grenade;
        private float _sourceVolume;
        private float _timeBeforeExplosion;

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
            base.Initialize(itemData);
            _granadeAudio = GetComponent<GranadeAudioPlayer>();
            _explosionManager = GetComponent<ExplosionManager>();
            _explosionSound = itemData.ActionClip;
            _explosionEffect = itemData.ExplosionEffect;
            _timeBeforeExplosion = itemData.TimeBeforeExplosion;
        }
    }
}