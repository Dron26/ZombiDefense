using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Location
{
    public class WorkPoint : MonoCache
    {
        public UnityAction<WorkPoint> OnClick;
        private bool _isBusy;
        private Collider _collider;

        public void OnMouseDown()
        {
            CheckState();
        }

        public void CheckState()
        {
            if (_isBusy == false)
            {
                OnClick?.Invoke(this);
            }
        }

        public void SetState(bool isActive)
        {
            _isBusy=isActive;
            _collider.enabled= !isActive;
        }
    }
}