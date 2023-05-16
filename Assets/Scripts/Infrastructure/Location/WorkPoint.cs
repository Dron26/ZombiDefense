using System;
using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Infrastructure.Location
{
    public class WorkPoint : MonoCache
    {
        [SerializeField] private GameObject _standartCircle;
        [SerializeField] private GameObject _improvedCircle;
        [SerializeField] private GameObject _expertCircle;
        [SerializeField] private GameObject _selectedCircle;
        private SpriteRenderer _currentCircle=new ();
        
        List<GameObject> _selectedCircles = new();
        public bool IsBusy=>_isBusy;
        public bool IsSelected => _isSelected;
        public int Level => _level;
        public UnityAction<WorkPoint> OnClick;
        public UnityAction<WorkPoint> OnSelected;
        
        private Humanoid _humanoid;
        private bool _isBusy;
        private bool _isSelected = false;
        private Collider _collider;
        private int _level;
        private float _upPercent;
        
        

        private void Awake()
        {
            _level = 0;
            
            _selectedCircles.Add(_standartCircle);
            _selectedCircles.Add(_improvedCircle);
            _selectedCircles.Add(_expertCircle);
            
            //_currentCircle.gameObject.SetActive(true);
            _collider=GetComponent<Collider>();

        }

        public void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            else
            {
                OnSelected?.Invoke(this);
              //  CheckState();
            }
           
        }

        public void SetBusy(bool isBusy)
        {
            _isBusy = isBusy;
        }

        public void SetSelected(bool isSelected)
        {
            _isSelected = isSelected;
            _selectedCircle.gameObject.SetActive(_isSelected);
        }

        public void CheckState()
        {
            // if (_isBusy == false)
            // {
            //     SetState(true);
            //    
            // }

            if (_isSelected==true&&_isBusy==true)
            {
                _humanoid.SetSelected(true);
            }
            else
            {
                
                OnClick?.Invoke(this);
            }
        }

        // public void SetState(bool isActive)
        // {
        //     _isBusy=isActive;
        //     _collider.enabled= !isActive;
        // }

        public void SetHumanoid(Humanoid humanoid)
        {
            _humanoid=humanoid;
            _humanoid.SetPontInfo();
            SetBusy(true);
        }

        public void UpLevel(float upPercent)
        {
            _selectedCircles[_level].gameObject.SetActive(false);
            _upPercent = upPercent;
            _level++;
            _selectedCircles[_level].gameObject.SetActive(true);
        } 
        
        public void LoadData()
        {
            
        }
    }
}