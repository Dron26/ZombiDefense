using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using Interface;
using Services;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
        public int Id => _id;
        private int _id;
        public int UpPrecent => _upPrecent;
        public float DefencePercent => _defencePercent;

        public bool IsHaveMedicineBox => _isHaveMedicineBox;
        public bool IsHaveWeaponBox => _isHaveWeaponBox;
        public bool _isHaveMedicineBox;

        private SpriteRenderer _currentCircle = new();
        private List<GameObject> _selectedCircles = new();
        private Character _character;
        private bool _isBusy;
        private bool _isSelected = false;
        private bool _isSelectedForMove = false;
        private Collider _collider;
        private int _level;
        private int _upPrecent;
        private MedicalKit _medicalKit;
        private AdditionalBox _weaponBox;
        private bool _isHaveWeaponBox;
        private ICharacterHandler _characterHandler;
        private  IWeaponController _weaponController;
        private float _defencePercent;
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

        public void SetMedicalBox<T>(AdditionalBox box, ref T boxField, ref bool isHaveBox) where T : EquipmentItem
        {
            // // Проверяем, что boxField еще не инициализирован
            // if (boxField != null)
            // {
            //     Debug.LogWarning("Medical box is already set.");
            //     return;
            // }
            //
            // // Ищем медицинский набор в коробке
            // EquipmentItem equipmentItem = box.GetItems().FirstOrDefault(item => item.Type == ItemType.MedicalKit);
            //
            // if (equipmentItem != null && equipmentItem is T specificBox)
            // {
            //     // Инициализируем boxField
            //     boxField = specificBox;
            //
            //     // Устанавливаем родительский объект и позицию
            //
            //     // Устанавливаем флаг наличия медицинского набора
            //     isHaveBox = true;
            // }
            // else
            // {
            //     Debug.LogWarning($"{typeof(T).Name} not found in the box.");
            // }
        }

        public T GetBox<T>(ref T boxField, ref bool isHaveBox) where T : EquipmentItem
        {
            isHaveBox = false;
            Destroy(boxField.gameObject, 0.1f);
            return boxField;
        }
        public T GetWeaponBox<T>(ref T boxField, ref bool isHaveBox) where T : AdditionalBox
        {
            isHaveBox = false;
            Destroy(boxField.gameObject, 0.1f);
            return boxField;
        }


        public void SetMedicineBox(AdditionalBox box)
        {
            SetMedicalBox(box, ref _medicalKit, ref _isHaveMedicineBox);
        }

        public MedicalKit GetMedicineBox()
        {
            return GetBox(ref _medicalKit, ref _isHaveMedicineBox);
        }

        public void SetWeaponBox(AdditionalBox weaponBox)
        {
            _weaponBox = weaponBox;
            Transform boxTransform = _weaponBox.transform;
            boxTransform.parent = transform;
            boxTransform.position = transform.position;
            _isHaveWeaponBox = true;
            CheckCharacter();
        }

        public AdditionalBox GetWeaponBox()
        {
            return GetWeaponBox(ref _weaponBox, ref _isHaveWeaponBox);
        }

        public void SetBusy(bool isBusy)
        {
            _isBusy = isBusy;
        }

        public void SetSelected(bool isSelected)
        {
            bool selected=_isSelected;
            _isSelected = isSelected;
            
            // if (selected==false)
            // {
            //     StartCoroutine(SelectedCircleActivated());
            // }
            // else 
            // {
            //     StopCoroutine(SelectedCircleActivated());
            // }
            
            _selectedCircle.gameObject.SetActive(_isSelected);
            
            if (_weaponController!=null)
            {
                _weaponController.SetSelected(_isSelected);
            }
        }

        private void SetWeaponController()
        {
            _weaponController = (IWeaponController)_character.GetComponent(typeof(IWeaponController));
            _weaponController.SetPoint(this);
            _weaponController.SetSelected(_isSelected);
        }

        public void CheckCharacter()
        {
            if (_character != null)
            {
                
                if (_isSelected)
                {
                    _characterHandler.SetSelectedCharacter(_character);
                    _character.SetPoint(this);
                    _isBusy = true;
                }
                
            }
            else
            {
                if (_isSelectedForMove == false)
                {
                    _isBusy = false;
                }
            }
        }

        public void SetCharacter(Character character)
        {
//            Debug.Log("SetCharacter");
            
            _character = character;
            _character.transform.parent = transform;
            SetWeaponController();
            CheckCharacter();
        }

        public void UpLevel(int procent)
        {
            
            _selectedCircles[_level].gameObject.SetActive(false);
            _upPrecent += procent;
            _level++;
            _selectedCircles[_level].gameObject.SetActive(true);
            CheckCharacter();
        }

        public void SetDefenceValue(float value)
        {
            _defencePercent = value;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PointerEventData newEventData= eventData;
            SetSelected(true);
            CheckCharacter();
            OnSelected?.Invoke(this);
            
        }

        public void Initialize(int id)
        {
            _characterHandler=AllServices.Container.Single<ICharacterHandler>();
            _id = id;
        }

        public void SelectedForMove(bool isSelected)
        {
            _isSelectedForMove = isSelected;
        }

        public void SetStartPointer()
        {
            SetSelected(true);
            CheckCharacter();
            OnSelected?.Invoke(this);
        }
        public void RemoveCharacter()
        {
            if (_character!=null)
            {
                _character.transform.SetParent(transform.parent, true);
                _character = null;
                _weaponController = null;
            }
            
            CheckCharacter();

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