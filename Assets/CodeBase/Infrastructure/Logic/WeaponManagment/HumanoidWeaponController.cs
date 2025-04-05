using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Humanoids.AbstractLevel;
using Characters.Robots;
using Data;
using Data.Upgrades;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.AssetManagement;
using Infrastructure.Location;
using Infrastructure.Logic.Inits;
using Interface;
using Services;
using UI.Buttons;
using UnityEngine;
using CharacterData = Characters.Humanoids.AbstractLevel.CharacterData;

namespace Infrastructure.Logic.WeaponManagment
{
    [RequireComponent(typeof(ObjectThrower))]
    public class HumanoidWeaponController : WeaponController, IWeaponController
    {
        [SerializeField] private SpriteRenderer _radius;
        [SerializeField] private WeaponContainer _weaponContainer;
        [SerializeField] private AttachmentSetter _attachmentSetter;
        public bool CanThrowGranade => _canThrowGranade;
        public int MaxAmmo =>_weapon.MaxAmmo;
        public float FireRate => _fireRate;
        public float Range =>_range;
        public float Damage =>_damage;

        public Action UpdateWeaponData;
        public Action<Weapon> OnInitialized;
        public Action OnChangeGranade;
        public float SpreadAngle => _spread;

        public Weapon GetActiveItemData() => _weapon;
        private Weapon _weapon;
        private GameObject _weaponPrefab;
        private ObjectThrower _objectThrower;
        private PlayerCharacterAnimController _playerCharacterAnimController;
        private Dictionary<int, float> _weaponAnimInfo = new();
        private List<Grenade> _granades = new();
        public int _damage;
        private float _damagePrecent;
        private float _reloadTime;
        private float _spread;
        private float _fireRate;
        private float _range;
        private float _increaseRangeValue;
        private int _rangeUp;
        private bool _isGranade;
        private bool _canThrowGranade;
        protected bool _isSelected;
        private BoxData _boxData;
        private List<BaseItem> _items;
        private IUpgradeHandler _upgradeHandler;
        private IUpgradeTree _upgradeTree;

        public void SetWeapon(Transform weaponTransform) =>
            _weaponPrefab.transform.parent = weaponTransform;

        private CharacterType _characterType;

        public override void Initialize(CharacterData data)
        {
            _objectThrower = GetComponent<ObjectThrower>();
            _objectThrower.OnThrowed += OnThrowedGranade;
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
            _playerCharacterAnimController.OnSetedAnimInfo += SetAnimInfo;
            _upgradeHandler= AllServices.Container.Single<IUpgradeHandler>();
            _upgradeTree=AllServices.Container.Single<IUpgradeTree>();
            
            SetWeapon(data);
            
            if (data.HaveAttachments)
            {
                SetAttachment(data);
            }

            SetRadius();
            OnInitialized?.Invoke(_weapon);
        }

        public void SetPoint(WorkPoint workPoint)
        {
            _damage = (int) Mathf.Round((_weapon.Damage * (1+(workPoint.UpPrecent+_damagePrecent) / 100)));
            _range = Mathf.RoundToInt(_increaseRangeValue * (1 + workPoint.UpPrecent / 100f));
            //SetShootingRadius();

            if (workPoint.IsHaveWeaponBox)
            {
                OpenWeaponBox(workPoint.GetWeaponBox());
            }
            
            SetRadius();
            UpdateWeaponData?.Invoke();
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
                _objectThrower.ThrowGrenade(_granades[0]);
            }
        }

        public void SetAdditionalWeaponButton(AdditionalWeaponButton additionalWeaponButton) =>
            additionalWeaponButton.OnClickButton += ThrowGranade;

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
            ItemType = data.ItemData.Type;
            string path = AssetPaths.ItemsData + ItemType;
            ItemData itemData = Resources.Load<ItemData>(path);
            _weaponContainer.SetItem(ItemType);
            _weapon = _weaponContainer.GetItem();
            _weapon.Initialize(itemData);
            _damage = _weapon.Damage;
            _range = _weapon.Range;
            _spread = _weapon.SpreadAngle;

            if (data.HaveWeaponLight)
            {
                Light = _weapon.Light;
                SetLight();
            }

            SetUpgrades();
           
            
            UpdateWeaponData?.Invoke();
        }

        private void SetLight()
        {
            Light.gameObject.SetActive(LighInformer.HasLight);
        }

        private void SetRadius() => _radius.transform.localScale = new Vector3(_range / 3.4f, _range / 3.4f, 1);


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
        
        private void SetUpgrades()
        {
            UpdateUpgradeValue(UpgradeGroupType.Weapons,UpgradeType.IncreaseDamage, value => _damagePrecent = value);
            _damage = (int) Mathf.Round((_damage * (1+(_damagePrecent) / 100)));
            UpdateUpgradeValue(UpgradeGroupType.Range,UpgradeType.IncreaseRange, value => _increaseRangeValue = value);
            _increaseRangeValue+=_range;
            SetRadius();
        }

        private void UpdateUpgradeValue(UpgradeGroupType groupType, UpgradeType type, Action<int> setValue)
        {
            var upgrades = _upgradeTree.GetUpgradeValue(groupType, type);
            if (upgrades != null && upgrades.Count > 0)
            {
                setValue((int)Mathf.Round(upgrades[0]));
            }
        }

        public void DamageLevelUp(float percent)
        {
            _damagePrecent= Mathf.RoundToInt(_damagePrecent * (1 + percent / 100));
        }
    }
}