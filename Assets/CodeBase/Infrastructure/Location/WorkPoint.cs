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
        public int UpPrecent=>_upPrecent;
        private SpriteRenderer _currentCircle=new ();
        private List<GameObject> _selectedCircles = new();
        private Humanoid _humanoid;
        private bool _isBusy;
        private bool _isSelected = false;
        private bool _isSelectedForMove = false;
        private Collider _collider;
        private int _level;
        private int _upPrecent;
        SaveLoadService _saveLoadService;

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
                _saveLoadService.SetSelectedHumanoid(_humanoid);
                _isBusy = true;
            }
            else
            { if (_isSelectedForMove==false)
                {
                    _isBusy = false;
                }
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

        public void UpLevel(int precent)
        {
            _selectedCircles[_level].gameObject.SetActive(false);
            _upPrecent += precent;
            _level++;
            _selectedCircles[_level].gameObject.SetActive(true);
        } 
        
        public void LoadData()
        {
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            print("Pointer");
            SetSelected(true);
            CheckState();
                OnSelected?.Invoke(this);
        }

        public void SetSaveLoad(SaveLoadService saveLoadService)
        {
            _saveLoadService=saveLoadService;
        }

        public void SelectedForMove(bool isSelected)
        {
            _isSelectedForMove=isSelected;
        }

        public void SetStartPointer()
        {
            print("Pointer");
            SetSelected(true);
            CheckState();
            OnSelected?.Invoke(this);
        }

        public void RemoveHumanoid()
        {
            
            _humanoid.transform.parent=transform.parent;
            CheckState();
        }
    }
}