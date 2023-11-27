using System;
using System.Collections.Generic;
using Data.Upgrades;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using Infrastructure.Location;

namespace Infrastructure.Logic.WeaponManagment
{
    public class WeaponController : MonoCache
    {
        private GameObject _weaponPrefab;

        [SerializeField] private Weapon _weapon;
        [SerializeField] private Weapon _granade;
        [SerializeField] private Sprite shootingRadiusSprite;
        [SerializeField] private ParticleSystem _ring;

        public Action ChangeWeapon;
        public Action<Weapon> OnInitialized;
        public Action OnAddGranade;
        public int CountGranade => _granades.Count;
        public int Damage => _damage;

        private PlayerCharacterAnimController _playerCharacterAnimController;
        private Animator _animator;
        private Humanoid _humanoid;
        private Dictionary<int, float> _weaponAnimInfo = new();
        private WeaponType _weaponWeaponType;
        private List<Granade> _granades = new();
        private int _damage;
        private int _maxAmmo;
        private float _reloadTime;
        private float _fireRate;
        private float _range;
        public float ReloadTime => _reloadTime;
        public bool _isShotgun;
        private bool _isGranade;

        public WeaponType WeaponWeaponType => _weaponWeaponType;
        public Weapon GetWeapon() => _weapon;
        
        public float GetRangeAttack() => _range;

        public Weapon GetActiveWeapon() => _weapon;

        public void SetWeapon(Transform weaponTransform) =>
            _weaponPrefab.transform.parent = weaponTransform;

        private GameObject _radiusObject;
        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            _humanoid = GetComponent<Humanoid>();
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
            _animator = GetComponent<Animator>();
        }

        public void Initialize()
        {
            SetAnimInfo();
            SetWeaponParametrs();
            ChangeWeapon?.Invoke();
            SetShootingRadiusSprite();
            SetShootingRadius();
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
            _weaponWeaponType = _weapon.GetWeaponType();
            _damage = _weapon.Damage;
            _maxAmmo = _weapon.MaxAmmo;
            _range = _weapon.Range;
        }

        private void SetShootingRadiusSprite()
        {
            _radiusObject= new GameObject("ShootingRadius");
            _radiusObject.SetActive(true);
            _radiusObject.transform.position = transform.position;
            _spriteRenderer= _radiusObject.AddComponent<SpriteRenderer>();
            _spriteRenderer.sprite = shootingRadiusSprite;
            SetShootingRadius();
        }
        private void SetShootingRadius()
        {
            _spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            _radiusObject.transform.localScale = new Vector3(_range * 2f, _range * 2f, 1f);
            _radiusObject.SetActive(false);
        }

        public int GetDamage() => _damage;

        public float GetSpread()
        {
            float spread = 8;
            return spread;
        }
        
        public float GetSpreadAngle() =>_weapon.SpreadAngle;

        public void SetDamage(int damage) => _damage += damage;
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
            _range=(_range * workPoint.UpPrecent) / 100;
            SetShootingRadius();
            
            if (workPoint.IsHaveWeaponBox)
            {
                OpenWeaponBox(workPoint.GetWeaponBox());
            }
        }

        private void OpenWeaponBox(WeaponBox weaponBox)
        {
            if (weaponBox.GetGranades().Count > 0)
            {
                gameObject.AddComponent<GrenadeThrower>();
                weaponBox.GetGranades().ForEach(granade => AddGranade(granade));
                OnAddGranade?.Invoke();
            }
            
        }

        public void AddGranade(Granade granade)
        {
            _granades.Add(granade);
            
        }

        public bool TryGetGranade(out Granade granade)
        {
            bool canGet = false;
            granade = null;
            
            if (_granades.Count > 0)
            {
                canGet = true;
                granade = GetGranade();
                return canGet;
            }
            else
            {
                Destroy(gameObject.GetComponent<GrenadeThrower>());
            }

            return canGet;
        }

        private Granade GetGranade()
        {
            Granade granade=_granades[0];
            _granades.RemoveAt(0);
            return granade;
        }
    }

    //    Sniper Vector3(0.0289999992,-0.0839999989,0.0170000009)Vector3(347.422821,89.5062866,89.6258698)
//    shotgun Vector3(0.0179999992,-0.0500000007,-0.0189999994)Vector3(348.677673,89.4979019,89.6276169)
}