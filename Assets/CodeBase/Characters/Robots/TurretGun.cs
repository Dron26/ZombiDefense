using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Characters.Robots
{
    public class TurretGun:MonoCache
    {
        public Action<Collider> OnEnter;
        public Action<Collider> OnExit;
        
        private void OnTriggerExit(Collider other)
        {
            OnExit?.Invoke(other);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            OnEnter?.Invoke(other);
        }
    }
}