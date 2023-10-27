using System.Collections;
using System.Timers;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    public class Granade : MonoCache
    {
        private ParticleSystem _explosion;
        private float _time = 3;
        private float _damageDistance;
        private int _damage;

        private void Initialize()
        {
            _explosion = GetComponent<ParticleSystem>();
            
        }
        
        private void StartTimer()
        {
            
            _time--;
        }   
        
        public void SetData()
        {
          _time = 3;
          _damageDistance = 5f;
          _damage = 50;
        }
        
        private   IEnumerator Timer()
        {
            while (_time! > 0)
            {
                _time -= Time.deltaTime;
                yield return null;
            }
            
            _explosion.Play();
        }
    }
}