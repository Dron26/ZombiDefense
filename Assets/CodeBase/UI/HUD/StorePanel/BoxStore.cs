using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using Infrastructure.AIBattle;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
using Interface;
using Services;
using Services.SaveLoad;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI.HUD.StorePanel
{
    
    [RequireComponent(typeof(BoxFactory))]
    [RequireComponent(typeof(ItemFactory))]
    
    public class BoxStore : MonoCache
    {
        [SerializeField] private AdditionalEquipment _additionalEquipmentButton;

        private BoxFactory _boxFactory;
        private ItemFactory _itemFactory;
        private WorkPoint _selectedWorkPoint;
        private Wallet _wallet;
        private Dictionary<BoxData, int> _boxesData=new Dictionary<BoxData, int>();
        private AdditionalBox _weaponBox;
        private AdditionalBox _medicineBox;

        private BoxType _currentWeaponBoxType;
        public event Action <BoxData> BuyBox;
        
        public void Initialize(Wallet wallet)
        {
            _wallet = wallet;
            LoadSettings(AllServices.Container.Single<IUpgradeHandler>().GetUpgradeData());
            _additionalEquipmentButton.Initialize();
            _boxFactory = GetComponent<BoxFactory>();
            _itemFactory= GetComponent<ItemFactory>();
            AddListener();
            SetBoxes();
        }

        private void SetBoxes()
        {
            List<BoxData> boxes=CreateBoxesGroup();
            InitializePrice(boxes);
        }

        private void InitializePrice(List<BoxData> boxes)
        {
            string path = AssetPaths.BoxesPrice;
            BoxesPrice boxesPrice = Resources.Load<BoxesPrice>(path);
            
            for (int i = 0; i < boxes.Count; i++)
            {
                if (boxesPrice != null && boxesPrice.BoxType.Contains(boxes[i].Type))
                {
                    _boxesData.Add(boxes[i], boxesPrice.Prices[boxesPrice.BoxType.IndexOf(boxes[i].Type)]);
                }
            }
        }

        private List<BoxData> CreateBoxesGroup()
        {   
            List<BoxData> boxes=new List<BoxData>();
            Object[] boxObjects = Resources.LoadAll(AssetPaths.Boxes);

            foreach (GameObject boxObject in boxObjects)
            {
                string fileName = Path.GetFileNameWithoutExtension(boxObject.name);


                if (Enum.TryParse(fileName, out BoxType boxType))
                { 
                    BoxData boxData = Resources.Load<BoxData>(AssetPaths.BoxesData + boxType);
                    boxes.Add(boxData);
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
            SelectBox(BoxType.Equipment);
        }

        private void SelectBox(BoxType type)
        {
            BoxData data = _boxesData.Keys.FirstOrDefault(box => box.Type == type);
                
            int price = _boxesData[data];

            if (_wallet.IsMoneyEnough(price))
            {
                _wallet.SpendMoney(price);
                BuyBox?.Invoke(data);
                _additionalEquipmentButton.HideButton();
            }
        }
        
        private void OnSelectSmallWeaponBox()
        {
            SelectBox(BoxType.SmallWeapon);
        }

        private void AddListener()
        {
            _additionalEquipmentButton.OnSelectedMedicineBox += OnSelectMedicineBox;
            _additionalEquipmentButton.OnSelectedWeaponBox += OnSelectSmallWeaponBox;
        }

        private void OnDestroy()
        {
            RemoveListener();
        }
        private void RemoveListener()
        {
            _additionalEquipmentButton.OnSelectedMedicineBox -= OnSelectMedicineBox;
            _additionalEquipmentButton.OnSelectedWeaponBox -= OnSelectSmallWeaponBox;
        }

        private void LoadSettings(GameParameters global)
        {
            _currentWeaponBoxType=global.WeaponBoxType;
        }
    }
}