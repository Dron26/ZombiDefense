using System;
using System.Collections.Generic;
using Data.Upgrades;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.Inits;
using UI.Buttons;
using UnityEngine;

namespace Infrastructure.Logic.WeaponManagment
{
    public abstract class WeaponController : MonoCache
    {
        private SpriteRenderer Radius;
        private GameObject WeaponContainer;
        public bool IsCanThrowGranade;
        public int Damage;
        public Action ChangeWeapon;
        public Action<Weapon> OnInitialized;
        public Action OnChangeGranade;
        public ItemType ItemType;
        public float GetRangeAttack;
        public float GetSpread;
        public Weapon GetActiveItemData;
        private Light Light;
        private ItemData ItemData;
        private Weapon Weapon;
        private GameObject WeaponPrefab;
        private GrenadeThrower GrenadeThrower;
        private PlayerCharacterAnimController PlayerCharacterAnimController;
        private Dictionary<int, float> WeaponAnimInfo = new();
        private List<Granade> Granades = new();
        private float _spread = 8;
        private int _damage;
        private float _reloadTime;
        private float _fireRate;
        private float _range;
        private bool _isGranade;
        private bool _isCanThrowGranade;
        protected bool _isSelected;


        public virtual void SetWeapon(Transform weaponTransform){}

        public virtual void Initialize(){}

        public float GetSpreadAngle() => ItemData.SpreadAngle;

        public void SetUpgrade(UpgradeData upgradeData, int level) => SetDamage(upgradeData.Damage);

        public void UIInitialize() => SetWeapon();

        public virtual void SetPoint(WorkPoint workPoint){}

        public virtual void SetSelected(bool isSelected){}
        public void AddGranade(List<Granade> granades) => Granades = new List<Granade>(granades);

        public virtual void ThrowGranade() {}

        public void SetAdditionalWeaponButton(AdditionalWeaponButton additionalWeaponButton) => additionalWeaponButton.OnClickButton += ThrowGranade;

        private void Awake() => _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();

        private void SetDamage(int damage) => _damage += damage;
        
        private void SetAnimInfo() {}

        private void SetWeapon(){}

        private void SetLight() => _light.gameObject.SetActive(LighInformer.HasLight);

        private void SetRadius() => _radius.transform.localScale=new Vector3(_range/3.6f, _range/3.6f, 1);
        private void OpenWeaponBox(AdditionalBox weaponBox) {}

        private void OnThrowedGranade() {}
    }
}