using System;
using Infrastructure.BaseMonoCache.Code.MonoCache.Interfaces;
using Infrastructure.BaseMonoCache.Code.System;
using UnityEngine.Device;

namespace Infrastructure.BaseMonoCache.Code.MonoCache
{
    public abstract class MonoCache : MonoShortСuts, IRunSystem, IFixedRunSystem, ILateRunSystem
    {
        private GlobalUpdate _globalUpdate;
        private bool _isSetup;
        
        private void OnEnable()
        {
            OnEnabled();

            if (_isSetup == false) 
                TrySetup();

            if (_isSetup) 
                SubscribeToGlobalUpdate();
        }

        protected virtual void OnDisable()
        {
            if (_isSetup) 
                UnsubscribeFromGlobalUpdate();

            OnDisabled();
        }

        private void TrySetup()
        {
            if (Application.isPlaying)
            {
                _globalUpdate = Singleton<GlobalUpdate>.Instance;
                _isSetup = true;
            }
            else
            {
                throw new Exception($"You tries to get {nameof(GlobalUpdate)} instance when application is not playing!");
            }
        }
        
        private void SubscribeToGlobalUpdate()
        {
            _globalUpdate.AddRunSystem(this);
            _globalUpdate.AddFixedRunSystem(this);
            _globalUpdate.AddLateRunSystem(this);
        }

        private void UnsubscribeFromGlobalUpdate()
        {
            _globalUpdate.RemoveRunSystem(this);
            _globalUpdate.RemoveFixedRunSystem(this);
            _globalUpdate.RemoveLateRunSystem(this);
        }

        void IRunSystem.OnRun() => 
            UpdateCustom();
        void IFixedRunSystem.OnFixedRun() => 
            FixedUpdateCustom();
        void ILateRunSystem.OnLateRun() => 
            LateUpdateCustom();

        protected virtual void OnEnabled() { }
        protected virtual void OnDisabled() { }
        
        protected virtual void UpdateCustom() { }
        protected virtual void FixedUpdateCustom() { }
        protected virtual void LateUpdateCustom() { }
    }
}
