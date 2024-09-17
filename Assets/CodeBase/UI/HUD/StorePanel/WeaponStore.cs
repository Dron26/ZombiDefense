using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Infrastructure.AIBattle;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoriesBox;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
using Service.SaveLoad;
using UI.Locations;
using UnityEngine;

namespace UI.HUD.StorePanel
{
    public class WeaponStore : MonoCache
    {
        [SerializeField] private AdditionalEquipment _additionalEquipmentButton;

        private SaveLoadService _saveLoadService;
        private BoxFactory _boxFactory;
        private WorkPoint _selectedWorkPoint;
        private Wallet _wallet;
        private Dictionary<AdditionalBox, int> _boxesData;
        
        private AdditionalBox _weaponBox;
        private AdditionalBox _medicineBox;

        public void Initialize(SaveLoadService saveLoadService, Wallet wallet)
        {
            _saveLoadService = saveLoadService;
            _wallet = wallet;
            _additionalEquipmentButton.Initialize(_saveLoadService);
            _boxFactory = GetComponent<BoxFactory>();

            SetBoxes();
        }

        private void SetBoxes()
        {
            List<AdditionalBox> boxes=CreateBoxesList();
            InitializePrice(boxes);
        }

        private void InitializePrice(List<AdditionalBox> boxes)
        {
            string path = AssetPaths.BoxesPrice;
            BoxesPrice boxesPrice = Resources.Load<BoxesPrice>(path);

            for (int i = 0; i < boxes.Count; i++)
            {
                if (boxesPrice != null && boxesPrice.BoxPrices.ContainsKey(boxes[i].GetType()))
                {
                    _boxesData.Add(_weaponBox, boxesPrice.BoxPrices[boxes[i].GetType()]);
                }
            }
        }

        private List<AdditionalBox> CreateBoxesList()
        {
            List<AdditionalBox> boxes=new List<AdditionalBox>();
            
            UnityEngine.Object[] boxObjects = Resources.LoadAll(AssetPaths.Boxes);

            foreach (var boxObject in boxObjects)
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(boxObject.name);

                if (Enum.TryParse(fileName, out BoxType boxType))
                {
                    boxes.Add(_boxFactory.Create(boxType));
                }
                else
                {
                    Debug.LogWarning($"Failed to parse BoxType from file name: {fileName}");
                }
            }
            
            return boxes; 
        }

        private void OnSelectMedicineBox()
        {
            if (!_selectedWorkPoint.IsHaveMedicineBox && !_selectedWorkPoint.IsHaveWeaponBox)
            {
                AdditionalBox medicineBox = _boxesData.Keys.FirstOrDefault(box => box.GetType() == BoxType.Equipment);

                if (medicineBox != null)
                {
                    int medicineBoxPrice = _boxesData[medicineBox];

                    if (_wallet.IsMoneyEnough(medicineBoxPrice))
                    {
                        _wallet.SpendMoney(medicineBoxPrice);
                        _selectedWorkPoint.SetMedicineBox(_boxFactory.Create(medicineBox.GetType()));
                        _additionalEquipmentButton.HideButton();
                    }
                }
            }
        }

        
        
        private void OnSelectWeaponBox()
        {
            if (!_selectedWorkPoint.IsHaveWeaponBox && !_selectedWorkPoint.IsHaveMedicineBox)
            {
                AdditionalBox weaponBox = _boxesData.Keys.FirstOrDefault(box => box.GetType() == BoxType.SmallWeapon);

                if (weaponBox != null)
                {
                    int weaponBoxPrice = _boxesData[weaponBox];

                    if (_wallet.IsMoneyEnough(weaponBoxPrice))
                    {
                        _wallet.SpendMoney(weaponBoxPrice);
                        _selectedWorkPoint.SetMedicineBox(_boxFactory.Create(weaponBox.GetType()));
                        _additionalEquipmentButton.HideButton();
                    }
                }
            }
        }

        private void OnSelectPoint(WorkPoint workPoint)
        {
            _selectedWorkPoint = workPoint;
        }

        private void AddListener()
        {
            _saveLoadService.OnSelectedNewPoint += OnSelectPoint;
            _additionalEquipmentButton.OnSelectedMedicineBox += OnSelectMedicineBox;
            _additionalEquipmentButton.OnSelectedWeaponBox += OnSelectWeaponBox;
        }

        private void RemoveListener()
        {
            _saveLoadService.OnSelectedNewPoint -= OnSelectPoint;

            _additionalEquipmentButton.OnSelectedMedicineBox -= OnSelectMedicineBox;
            _additionalEquipmentButton.OnSelectedWeaponBox -= OnSelectWeaponBox;
        }
    }
}