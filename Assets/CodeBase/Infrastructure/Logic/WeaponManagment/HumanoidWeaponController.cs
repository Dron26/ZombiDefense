using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Upgrades;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.AssetManagement;
using Infrastructure.Location;
using Infrastructure.Logic.Inits;
using UI.Buttons;
using UnityEngine;


namespace Infrastructure.Logic.WeaponManagment
{
    
    [RequireComponent(typeof(GrenadeThrower))]

    public class HumanoidWeaponController : WeaponController,IWeaponController
    {

        [SerializeField] private ItemType _itemType;
        [SerializeField] private SpriteRenderer _radius;
        [SerializeField] private GameObject _weaponContainer;
        public bool CanThrowGranade => _canThrowGranade;
        public Action ChangeWeapon;
        public Action<Weapon> OnInitialized;
        public Action OnChangeGranade;
        public float GetRangeAttack() => _range;
        public float GetSpread() => _spread;

        public Weapon GetActiveItemData() => _weapon;
        private Weapon _weapon;
        private GameObject _weaponPrefab;
        private GrenadeThrower _grenadeThrower;
        private PlayerCharacterAnimController _playerCharacterAnimController;
        private Dictionary<int, float> _weaponAnimInfo = new();
        private List<Grenade> _granades = new();
        public int _damage;
        private float _reloadTime;
        private float _spread = 8;
        private float _fireRate;
        private float _range;
        private bool _isGranade;
        private bool _canThrowGranade;
        protected bool _isSelected;
        private BoxData _boxData;
        private List<BaseItem> _items;
        public void SetWeapon(Transform weaponTransform) =>
            _weaponPrefab.transform.parent = weaponTransform;

        public override void Initialize()
        {
            SetAnimInfo();
            SetWeapon();
            SetLight();
            SetRadius();

            OnInitialized?.Invoke(_weapon);
        }

        public float GetSpreadAngle() => ItemData.SpreadAngle;

        public void SetUpgrade(UpgradeData upgradeData, int level) => SetDamage(upgradeData.Damage);

        public void UIInitialize() => SetWeapon();

        public void SetPoint(WorkPoint workPoint)
        {
            _damage = (_damage * workPoint.UpPrecent) / 100;
            _range = (_range * workPoint.UpPrecent) / 100;
            //SetShootingRadius();

            if (workPoint.IsHaveWeaponBox)
            {
                OpenWeaponBox(workPoint.GetWeaponBox());
            }
        }

        public void SetSelected(bool isSelected)
        {
            if (_radius != null)
            {
                _radius.gameObject.SetActive(isSelected);
            }
        }

        public void ThrowGranade()
        {
            if (_granades.Count > 0)
            {
                _canThrowGranade = false;
                _grenadeThrower.ThrowGrenade(_granades[0]);
            }
        }

        public void SetAdditionalWeaponButton(AdditionalWeaponButton additionalWeaponButton) => additionalWeaponButton.OnClickButton += ThrowGranade;

        private void Awake()
        {
            _grenadeThrower = GetComponent<GrenadeThrower>();
            _grenadeThrower.OnThrowed += OnThrowedGranade;
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
        }

        private void SetDamage(int damage) => _damage += damage;
        
        private void SetAnimInfo()
        {
            foreach (KeyValuePair<int, float> info in _playerCharacterAnimController.GetAnimInfo())
            {
                _weaponAnimInfo.Add(info.Key, info.Value);
            }
        }

        private void SetWeapon()
        {
            string path = AssetPaths.ItemsData + _itemType;
            ItemData itemData = Resources.Load<ItemData>(path);

            path = AssetPaths.WeaponPrefabs + _itemType;
            GameObject weapon = Instantiate(Resources.Load<GameObject>(path),_weaponContainer.transform);
            
            //Prefab/Store/Items/WeaponPrefabs/Pistol
            
            _weapon =  weapon.GetComponent<Weapon>();
            _weapon.Initialize(itemData);
            Light = _weapon.GetWeaponLigt;
            Damage = _weapon.Damage;
            _range = _weapon.Range;
            
            ChangeWeapon?.Invoke();
        }

        private void SetLight() => Light.gameObject.SetActive(LighInformer.HasLight);

        private void SetRadius() => _radius.transform.localScale=new Vector3(_range/3.6f, _range/3.6f, 1);


      
        private void OpenWeaponBox(AdditionalBox weaponBox)
        {
            _boxData = weaponBox.GetData();
            _items = weaponBox.GetItems();
            foreach (var item in _items)
            {
                item.transform.SetParent(transform); 
            }
            List<Grenade> granades = _items.OfType<Grenade>().ToList();

            if (granades.Count > 0)
            {
                foreach (Grenade grenade in granades)
                {
                    _granades.Add(grenade);
                }
                
                _canThrowGranade = true;
                OnChangeGranade?.Invoke();
            }
        }


        private void OnThrowedGranade()
        {
            _granades.RemoveAt(0);

            if (_granades.Count != 0)
            {
                _canThrowGranade = true;
            }
            else
            {
                _canThrowGranade = false;
            }

            OnChangeGranade?.Invoke();
        }
    }
}