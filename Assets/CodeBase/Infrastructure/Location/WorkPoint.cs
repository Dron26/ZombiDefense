using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Infrastructure.Location
{
    public class WorkPoint : MonoCache, IPointerClickHandler
    {
        [SerializeField] private GameObject _standartCircle;
        [SerializeField] private GameObject _improvedCircle;
        [SerializeField] private GameObject _expertCircle;
        [SerializeField] private GameObject _selectedCircle;

        public UnityAction<WorkPoint> OnSelected;
        public Action OnClick;
        public bool IsBusy => _isBusy;
        public bool IsSelected => _isSelected;
        public int Level => _level;
        public int UpPrecent => _upPrecent;

        public bool IsHaveMedicineBox => _isHaveMedicineBox;
        public bool IsHaveWeaponBox => _isHaveWeaponBox;
        public bool _isHaveMedicineBox;

        private SpriteRenderer _currentCircle = new();
        private List<GameObject> _selectedCircles = new();
        private Humanoid _humanoid;
        private bool _isBusy;
        private bool _isSelected = false;
        private bool _isSelectedForMove = false;
        private Collider _collider;
        private int _level;
        private int _upPrecent=100;
        SaveLoadService _saveLoadService;
        private MedicineBox _medicineBox;
        private WeaponBox _weaponBox;
        private bool _isHaveWeaponBox;
        
        private Vector3 _startScale;
        private Vector3 _maxScale;
        private Vector3 _minScale = new Vector3(0.3f,0.3f,0.3f);
        private void Awake()
        {
            _level = 0;

            _selectedCircles.Add(_standartCircle);
            _selectedCircles.Add(_improvedCircle);
            _selectedCircles.Add(_expertCircle);

            //_currentCircle.gameObject.SetActive(true);
            _collider = GetComponent<Collider>();
            
            _startScale = _selectedCircle.transform.localScale;
            _maxScale =_startScale*2 ;
        }


        public void SetMedicineBox(MedicineBox medicineBox)
        {
            _medicineBox = medicineBox;
            Transform medicineTransform = _medicineBox.transform;
            medicineTransform.parent = transform;
            medicineTransform.position = transform.position;
            _isHaveMedicineBox = true;
        }

        public MedicineBox GetMedicineBox()
        {
            _isHaveMedicineBox = false;
            Destroy(_medicineBox.gameObject, 0.1f);
            return _medicineBox;
        }

        public void SetActiveMedicineBox(bool isActive)
        {
            _medicineBox.gameObject.SetActive(isActive);
        }

        public void SetWeaponBox(WeaponBox weaponBox)
        {
            _weaponBox = weaponBox;
            Transform weaponTransform = _weaponBox.transform;
            weaponTransform.parent = transform;
            weaponTransform.position = transform.position;
            _isHaveWeaponBox = true;
            CheckState();
        }

        public WeaponBox GetWeaponBox()
        {
            _isHaveWeaponBox = false;
            Destroy(_weaponBox.gameObject, 0.1f);
            return _weaponBox;
        }

        private void SetActiveWeaponBox(bool isActive)
        {
            _weaponBox.gameObject.SetActive(isActive);
        }


        public void SetBusy(bool isBusy)
        {
            _isBusy = isBusy;
        }

        public void SetSelected(bool isSelected)
        {
            bool selected=_isSelected;
            _isSelected = isSelected;
            
            if (selected==false)
            {
                StartCoroutine(SelectedCircleActivated());
            }
            else 
            {
                StopCoroutine(SelectedCircleActivated());
            }
            
           
            _selectedCircle.gameObject.SetActive(_isSelected);
        }

        public void CheckState()
        {
            //_humanoid = GetComponentInChildren<Humanoid>();

            if (_humanoid != null)
            {
                _humanoid.SetSelected(true);
                _saveLoadService.SetSelectedHumanoid(_humanoid);
                _humanoid.SetPoint(this);
                _humanoid.GetComponent<WeaponController>().SetPoint(this);
                _isBusy = true;
               // CheckBoxes();
            }
            else
            {
                if (_isSelectedForMove == false)
                {
                    _isBusy = false;
                }
            }
        }

        public void SetHumanoid(Humanoid humanoid)
        {
            Debug.Log("SetHumanoid");
            
            _humanoid = humanoid;
            _humanoid.transform.parent = transform;
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
                SetSelected(true);
                CheckState();
                OnSelected?.Invoke(this);
            
        }

        public void SetSaveLoad(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public void SelectedForMove(bool isSelected)
        {
            _isSelectedForMove = isSelected;
        }

        public void SetStartPointer()
        {
            SetSelected(true);
            CheckState();
            OnSelected?.Invoke(this);
        }

        // private void CheckBoxes()
        // {
        //     bool isActive = _humanoid == null;
        //
        //     if (_isHaveMedicineBox)
        //         SetActiveMedicineBox(isActive);
        //     if (_isHaveWeaponBox)
        //         SetActiveWeaponBox(isActive);
        // }

        public void RemoveHumanoid()
        {
            
            
            if (_humanoid==null)
            {
                Debug.Log("RemoveHumanoid");
            }
            _humanoid.transform.parent = transform.parent;
            _humanoid = null;
            //CheckBoxes();
            CheckState();
        }


        private IEnumerator SelectedCircleActivated()
        {
            _selectedCircle.transform.localScale = _startScale;
            bool isUp= true;
            bool isDown= false;
            
            while (_isSelected)
            {
                if (isUp)
                {
                    while (isUp)
                    {
                        if (_selectedCircle.transform.localScale.x<_maxScale.x)
                        {
                            float time = Time.deltaTime;
                            _selectedCircle.transform.localScale+=time*(_selectedCircle.transform.localScale);
                        }
                        else
                        {
                            isUp=false;
                            isDown=true;
                        }
                        
                        yield return null;
                    }
                   
                }
                else
                {
                    while (isDown)
                    {
                        if (_selectedCircle.transform.localScale.x>_minScale.x)
                        {
                            float time = Time.deltaTime;
                            _selectedCircle.transform.localScale-=time*(_selectedCircle.transform.localScale);
                        }
                        else
                        {
                            isDown=false;
                            isUp=true;
                        }
                        yield return null;
                    }
                }
               
                yield return null;
                
            }
            
            _selectedCircle.transform.localScale = _startScale;
        }
    }
}