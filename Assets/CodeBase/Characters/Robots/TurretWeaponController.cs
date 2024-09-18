using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Humanoids.AbstractLevel;
using Data.Upgrades;
using Infrastructure.AIBattle;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
using UI.Buttons;
using Unity.VisualScripting;
using UnityEngine;

namespace Characters.Robots
{
    public class TurretWeaponController : MonoCache, IWeaponController
    {
        private GameObject _weaponPrefab;

        [SerializeField] private Weapon _weapon;

        public Action ChangeWeapon;
        public Action<Weapon> OnInitialized;
        public Action OnChangeGranade;
        public int CountGranade => _granades.Count;
        public int Damage
        {
            get => _damage;
            set => _damage = value;
        }
        public bool IsCanThrowGranade => _isCanThrowGranade;
        private GrenadeThrower _grenadeThrower;
        private PlayerCharacterAnimController _playerCharacterAnimController;
        private Animator _animator;
        private Humanoid _humanoid;
        private Dictionary<int, float> _weaponAnimInfo = new();
        private ItemType _itemItemType;
        private List<Granade> _granades = new();
        private int _damage;
        private int _maxAmmo;
        private float _reloadTime;
        private float _fireRate;
        private float _range;
        public float ReloadTime => _reloadTime;
        public bool _isShotgun;
        private bool _isGranade;
        private bool _isCanThrowGranade;
        public ItemType ItemItemType => _itemItemType;
        public Weapon GetWeapon() => _weapon;
        public bool IsSelected => _isSelected;
        protected bool _isSelected;
        public float GetRangeAttack() => _range;
    private TurretStateMachine _stateMachine;
        public Weapon GetActiveWeapon() => _weapon;
public TurretGun GetActiveTurretGun() => _turretGun;
private TurretGun _turretGun;
        public void SetWeapon(Transform weaponTransform) =>
            _weaponPrefab.transform.parent = weaponTransform;

        private GameObject _radiusObject;
        private SpriteRenderer _spriteRenderer;
        public Action OnSelected;
        public void Initialize()
        {
            
            _humanoid = GetComponent<Humanoid>();
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
            _animator = GetComponent<Animator>();
            _stateMachine = GetComponent<TurretStateMachine>();
            _turretGun=transform.GetComponentInChildren<TurretGun>();
          //  SetAnimInfo();
            SetWeaponParametrs();
            ChangeWeapon?.Invoke();
            SetShootingRadiusSprite();
            OnInitialized?.Invoke(_weapon);
        }
       
        private void SetGranadeParametrs()
        {
            if (!_isGranade)
            {
                _fireRate = _weaponAnimInfo[_playerCharacterAnimController.IsShoot];
                _reloadTime = _weaponAnimInfo[_playerCharacterAnimController.Reload];
            }
        }

        private void SetAnimInfo()
        {
            foreach (KeyValuePair<int, float> info in _playerCharacterAnimController.GetAnimInfo())
            {
                _weaponAnimInfo.Add(info.Key, info.Value);
            }
        }

        private void SetWeaponParametrs()
        {
            _itemItemType = _weapon.ItemType;
            _damage = _weapon.Damage;
            _maxAmmo = _weapon.MaxAmmo;
            _range = _weapon.Range;
        }

        private void SetShootingRadiusSprite()
        {
            _radiusObject = new GameObject("ShootingRadius");
            _radiusObject.SetActive(true);
            _radiusObject.transform.position = transform.position;
            _spriteRenderer = _radiusObject.AddComponent<SpriteRenderer>();
          
        }

        public int GetDamage() => _damage;

        public float GetSpread()
        {
            float spread = 8;
            return spread;
        }

        public float GetSpreadAngle() => _weapon.SpreadAngle;

        private void SetDamage(int damage) => _damage += damage;

        public void SetUpgrade(UpgradeData upgradeData, int level)
        {
            SetDamage(upgradeData.Damage);
        }

        public void UIInitialize()
        {
            SetWeaponParametrs();
        }

        public void SetPoint(WorkPoint workPoint)
        {
            _damage = (_damage * workPoint.UpPrecent) / 100;
            _range = (_range * workPoint.UpPrecent) / 100;

            if (workPoint.IsHaveWeaponBox)
            {
                OpenWeaponBox(workPoint.GetWeaponBox());
            }
        }

        public void SetSelected(bool isSelected)
        {
            _isSelected = isSelected;
            OnSelected?.Invoke();
        }

        private void OpenWeaponBox(AdditionalBox weaponBox)
        {
            List<Granade> granades = weaponBox.GetItems().OfType<Granade>().ToList();

            if (granades.Count > 0)
            {
                gameObject.AddComponent<GrenadeThrower>();
                _grenadeThrower = GetComponent<GrenadeThrower>();
                _grenadeThrower.OnThrowed += OnThrowedGranade;
                AddGranade(granades);
                _isCanThrowGranade = true;
                OnChangeGranade?.Invoke();
            }
        }

        private void OnThrowedGranade()
        {

            _granades.RemoveAt(0);

            if (_granades.Count != 0)
            {
                _isCanThrowGranade = true;
            }
            else
            {
                _isCanThrowGranade = false;
                Destroy(gameObject.GetComponent<GrenadeThrower>(), 3f);
            }

            OnChangeGranade?.Invoke();
            
        }

        public void AddGranade(List<Granade> granades)
        {
            _granades = new List<Granade>(granades);
        }

        public void ThrowGranade()
        {
            if (_granades.Count > 0)
            {
                _isCanThrowGranade = false;
                _grenadeThrower.ThrowGrenade(_granades[0]);
            }
        }

        public void SetAdditionalWeaponButton(AdditionalWeaponButton additionalWeaponButton)
        {
            additionalWeaponButton.OnClickButton += ThrowGranade;
        }
    }
}