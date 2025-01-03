using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Humanoids.AbstractLevel;
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
        [SerializeField] private SpriteRenderer _radius;
        [SerializeField] private WeaponContainer _weaponContainer;
        [SerializeField] private AttachmentSetter _attachmentSetter;
        public bool CanThrowGranade => _canThrowGranade;
        public Action ChangeWeapon;
        public Action<Weapon> OnInitialized;
        public Action OnChangeGranade;
        public float GetRangeAttack() => _weapon.Range;
        public float GetSpreadAngle() => _spread;

        public Weapon GetActiveItemData() => _weapon;
        private Weapon _weapon;
        private GameObject _weaponPrefab;
        private GrenadeThrower _grenadeThrower;
        private PlayerCharacterAnimController _playerCharacterAnimController;
        private Dictionary<int, float> _weaponAnimInfo = new();
        private List<Grenade> _granades = new();
        public int _damage;
        private float _reloadTime;
        private float _spread ;
        private float _fireRate;
        private float _range;
        private bool _isGranade;
        private bool _canThrowGranade;
        protected bool _isSelected;
        private BoxData _boxData;
        private List<BaseItem> _items;
        public void SetWeapon(Transform weaponTransform) =>
            _weaponPrefab.transform.parent = weaponTransform;

        private CharacterType _characterType;
        

        public override void Initialize(CharacterData data)
        {
            _grenadeThrower = GetComponent<GrenadeThrower>();
            _grenadeThrower.OnThrowed += OnThrowedGranade;
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
            _playerCharacterAnimController.OnSetedAnimInfo+=SetAnimInfo;
            
            SetWeapon(data);
            
            if (data.HaveAttachments)
            {
                SetAttachment(data);
            }
            
            SetRadius();

            OnInitialized?.Invoke(_weapon);
        }
        public void SetUpgrade(UpgradeData upgradeData, int level) => SetDamage(upgradeData.Damage);
        
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

        private void SetDamage(int damage) => _damage += damage;
        
        private void SetAnimInfo()
        {
            
            foreach (KeyValuePair<int, float> info in _playerCharacterAnimController.GetAnimInfo())
            {
                _weaponAnimInfo.Add(info.Key, info.Value);
            }
        }

        private void SetWeapon(CharacterData data)
        {
            ItemType=data.ItemData.Type;
            string path = AssetPaths.ItemsData + ItemType;
            ItemData itemData = Resources.Load<ItemData>(path);

            if (data.HaveWeaponLight)
            {
                SetLight();
            }
            
            _weaponContainer.SetItem(ItemType);
           
            //path = AssetPaths.WeaponPrefabs + _itemType;
           // GameObject weapon = Instantiate(Resources.Load<GameObject>(path),_weaponContainer.transform);
            
            //Prefab/Store/Items/WeaponPrefabs/Pistol
            //_weapon =  weapon.GetComponent<Weapon>();
            _weapon =  _weaponContainer.GetItem();
            _weapon.Initialize(itemData);
            Damage = _weapon.Damage;
            _range = _weapon.Range;
            _spread= _weapon.SpreadAngle;
            ChangeWeapon?.Invoke();
        }

        private void SetLight()
        {
                Light.gameObject.SetActive(LighInformer.HasLight);
        }

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

        private void SetAttachment(CharacterData data)
        {
            _characterType = data.Type;
            _attachmentSetter.SetAttachments(_characterType);
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