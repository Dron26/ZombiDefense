using System;
using Characters.Humanoids.AbstractLevel;
using Data.Upgrades;
using Infrastructure.AIBattle;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
using Interface;
using Services;
using UnityEngine;

namespace Characters.Robots
{
    public class TurretWeaponController : WeaponController, IWeaponController
    {
        private GameObject _weaponPrefab;
        [SerializeField] private Weapon _weapon;
        public Action ChangeWeapon;
        public Action<Weapon> OnInitialized;

        public int Damage
        {
            get => _damage;
            set => _damage = value;
        }
        private int _damage;
        private float _reloadTime;
        private float _precentAdditionalDamage;
        private float _fireRate;
        private float _range;
        private bool _isGranade;
        private bool _isCanThrowGranade;
        private bool _isCarTurret=false;
        private bool _isAimAdd=false;
        
        public Weapon GetWeapon() => _weapon;
        public bool IsSelected => _isSelected;
        protected bool _isSelected;
        public TurretGun GetActiveTurretGun() => _turretGun;
        private TurretGun _turretGun;
        private GameObject _radiusObject;
        public Action OnSelected;
        private ObjectThrower _objectThrower;
        private IUpgradeTree _upgradeTree;
        public override void Initialize(CharacterData data)
        {
            _turretGun = transform.GetComponentInChildren<TurretGun>();
            SetWeaponParametrs(data);
            ChangeWeapon?.Invoke();
            SetShootingRadiusSprite();
            SetUpgrades();
            _upgradeTree = AllServices.Container.Single<IUpgradeTree>();
            AllServices.Container.Single<IGameEventBroadcaster>().OnActivatedSpecialTechnique += ActiveTurret;
            //  OnInitialized?.Invoke(_weapon);
        }

        private void ActiveTurret()
        {
            _isCarTurret = true;
        }

        private void SetWeaponParametrs(CharacterData data)
        {
            _weapon.Initialize(data.ItemData);
            ItemType = _weapon.ItemType;
            _damage = _weapon.Damage;
            _range = _weapon.Range;
        }

        private void SetShootingRadiusSprite()
        {
            _radiusObject = new GameObject("ShootingRadius");
            _radiusObject.SetActive(true);
            _radiusObject.transform.position = transform.position;
        }

        public float GetSpread()
        {
            float spread = 8;
            return spread;
        }

        private void SetDamage(int damage) => _damage += damage;
        
        public void SetPoint(WorkPoint workPoint)
        {
            _damage= Mathf.RoundToInt(_damage * (1 + workPoint.UpPrecent / 100));
            _range= Mathf.RoundToInt(_range * (1 + workPoint.UpPrecent / 100));
        }

        public void SetSelected(bool isSelected)
        {
            _isSelected = isSelected;
            OnSelected?.Invoke();
        }
        
        private void SetUpgrades()
        {
            
            if (_isCarTurret)
            {
                UpdateUpgradeValue(UpgradeGroupType.SpecialTechnique, UpgradeType.IncreaseDamageSpecialTechnique, value => _precentAdditionalDamage = value);
            }
            else
            {
                UpdateUpgradeValue(UpgradeGroupType.Turrets, UpgradeType.IncreaseDamageSpecialTechnique, value => _precentAdditionalDamage = value);
            }
            
            _damage= Mathf.RoundToInt(_damage * (1 + _precentAdditionalDamage / 100));
        }

        private void UpdateUpgradeValue(UpgradeGroupType groupType, UpgradeType type, Action<int> setValue)
        {
            var upgrades = _upgradeTree.GetUpgradeValue(groupType, type);
            if (upgrades != null && upgrades.Count > 0)
            {
                setValue((int)Mathf.Round(upgrades[0]));
            }
        }
    }
}