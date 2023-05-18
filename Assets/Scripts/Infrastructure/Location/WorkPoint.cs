using System;
using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Infrastructure.Location
{
    public class WorkPoint : MonoCache,IPointerClickHandler
    {
        [SerializeField] private GameObject _standartCircle;
        [SerializeField] private GameObject _improvedCircle;
        [SerializeField] private GameObject _expertCircle;
        [SerializeField] private GameObject _selectedCircle;
        
        public UnityAction<WorkPoint> OnSelected;
        public UnityAction<WorkPoint> OnClick;
        public bool IsBusy=>_isBusy;
        public bool IsSelected => _isSelected;
        public int Level => _level;
        
        private SpriteRenderer _currentCircle=new ();
        private List<GameObject> _selectedCircles = new();
        private Humanoid _humanoid;
        private bool _isBusy;
        private bool _isSelected = false;
        private Collider _collider;
        private int _level;
        private float _upPercent;
        SaveLoad _saveLoad;
        

        private void Awake()
        {
            _level = 0;
            
            _selectedCircles.Add(_standartCircle);
            _selectedCircles.Add(_improvedCircle);
            _selectedCircles.Add(_expertCircle);
            
            //_currentCircle.gameObject.SetActive(true);
            _collider=GetComponent<Collider>();
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
            _humanoid = GetComponentInChildren<Humanoid>();
            
            if (_humanoid != null)
            {
                _humanoid.SetSelected(true);
                _saveLoad.SetSelectedHumanoid(_humanoid);
                _isBusy = true;
            }
            else
            {
                _isBusy = false;
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
            _humanoid.transform.parent=transform;
            CheckState();
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

        public void OnPointerClick(PointerEventData eventData)
        {
            SetSelected(true);
            CheckState();
                OnSelected?.Invoke(this);
        }

        public void SetSaveLoad(SaveLoad saveLoad)
        {
            _saveLoad=saveLoad;
        }
    }
}