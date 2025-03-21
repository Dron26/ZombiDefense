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
    
    public class BoxStore : MonoCache
    {
        [SerializeField] private AdditionalEquipment _additionalEquipmentButton;

        private BoxFactory _boxFactory;
        private WorkPoint _selectedWorkPoint;
        private Wallet _wallet;
        private Dictionary<BoxData, int> _boxesData=new Dictionary<BoxData, int>();
        private AdditionalBox _weaponBox;
        private AdditionalBox _medicineBox;
        private int DefaultGranadeCountInBox;
        private Upgrade _upgrade;
        public event Action <BoxData> BuyBox;
        private IUpgradeHandler _upgradeHandler;
        private IUpgradeTree _upgradeTree;
        private BoxType _currentType=0;
        public void Initialize(Wallet wallet)
        {
            _wallet = wallet;
            _additionalEquipmentButton.Initialize();
            _boxFactory = GetComponent<BoxFactory>();
            _upgradeTree=AllServices.Container.Single<IUpgradeTree>();
            AddListener();
            SetBoxes();
            SetUpgrades();
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
            Object[] boxDatas = Resources.LoadAll<BoxData>(AssetPaths.BoxesData);

            foreach (BoxData box in boxDatas)
            {
                
                    boxes.Add(box);
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
            SelectBox(BoxType.Granade);
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

        private void SetUpgrades()
        {
            List<float> values=_upgradeTree.GetUpgradeValue(UpgradeGroupType.Box,UpgradeType.AddGrenadeInBox);

            _currentType = (BoxType)(int)values[0];

        }
    }
}