using System;
using Data.Upgrades;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
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
        private float _fireRate;
        private float _range;
        private bool _isGranade;
        private bool _isCanThrowGranade;
        public Weapon GetWeapon() => _weapon;
        public bool IsSelected => _isSelected;
        protected bool _isSelected;
        public TurretGun GetActiveTurretGun() => _turretGun;
        private TurretGun _turretGun;
        private GameObject _radiusObject;
        public Action OnSelected;
        private GrenadeThrower _grenadeThrower;

        public override void Initialize()
        {
            _turretGun = transform.GetComponentInChildren<TurretGun>();
            SetWeaponParametrs();
            ChangeWeapon?.Invoke();
            SetShootingRadiusSprite();
            OnInitialized?.Invoke(_weapon);
        }

        private void SetWeaponParametrs()
        {
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

        public void SetUpgrade(UpgradeData upgradeData, int level)
        {
            SetDamage(upgradeData.Damage);
        }

        public void SetPoint(WorkPoint workPoint)
        {
            _damage = (_damage * workPoint.UpPrecent) / 100;
            _range = (_range * workPoint.UpPrecent) / 100;
        }

        public void SetSelected(bool isSelected)
        {
            _isSelected = isSelected;
            OnSelected?.Invoke();
        }
    }
}