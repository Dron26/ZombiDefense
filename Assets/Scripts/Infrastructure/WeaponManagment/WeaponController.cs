using System.Collections.Generic;
using System.Linq;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.WeaponManagment
{
    public class WeaponController : MonoCache, IWeapon
    {
        private WeaponData _weaponData;
        private GameObject _weaponPrefab;
        
        private Weapon _smallArms;
        private Weapon _granade;
        
         [SerializeField]private GameObject _smallArmsGameObject;
        [SerializeField] private Type _smallArmsType;
         [SerializeField]private GameObject _granadeGameObject;
         [SerializeField] private Type _garnadeType;
         
        private Weapon _weapon;
        private Weapon _weaponPistol;
        private Weapon _weaponRifle;
        private Weapon _weaponSniperRifle;
        private Weapon _weaponHeavyMachineGun;
        private Weapon _weaponGranade;
        private AnimController _animController;
        private Animator _animator;
        //private  Weapon _weaponGranade;
        private int _numberSmallArms = 0;
        private int _numberGranade = 4;
        private Humanoid _humanoid;
        private float _fireTimer = 0f;
        private int _ammoCount;
        private int _totalReceivedDamage;
        private Dictionary<int, float> _weaponAnimInfo=new();

        private string _weaponName;
        private int _damage;
        private int _maxAmmo;
        private float _reloadTime;
        private float _fireRate;
        private float _range;


        public UnityAction ChangeWeapon;
        public float ReloadTime => _reloadTime;
        public bool IsShotgun;
        public void Initialize()
        {
             _humanoid=GetComponent<Humanoid>();
            _weaponData = _humanoid.GetWeaponData();
            
            _animController=GetComponent<AnimController>();
            _animator=GetComponent<Animator>();

            SetWeapons();
            SetAnimInfo();
            SetWeaponParametrs();
            SetSmallArmsPrefab();
            ChangeWeapon?.Invoke();
        }
        
        public void SetSmallArmsPrefab()
        {
            _smallArmsGameObject.SetActive(true);
            
        }
        
        public void SetGranadePrefab()
        {
            _granadeGameObject.SetActive(true);
            
        }
        
        public WeaponData GetWeaponData()
        {
            return _weaponData;
        }
        
        public Weapon GetSmallArms()
        {
            return _smallArms;
        }
        
        public Weapon GetGranads()
        {
            return _granade;
        }

        private void SetWeapons()
        { 
            List<Weapon> smallArms=_weaponData.SmallArms();
            List<Weapon> granads=_weaponData.Granads();

            if (smallArms!=null)
            {
                for (int i = 0; i < smallArms.Count; i++)
                {
                    if (smallArms[i]!=null)
                    {
                        _smallArms=smallArms[i];
                        IsShotgun=_smallArms.IsShotgun;
                    }
                }
            }
            else
            {
                _smallArms=new ();
                print("Send null smallArms in Prefab");
            }
           
            if (granads != null)
            {
                for (int i = 0; i < granads.Count; i++)
                {
                    if (granads[i]!=null)
                    {
                        _granade=granads[i];
                    }
                }
            }
            else
            {
                granads=new ();
                print("Send null granads in Prefab");
            }
            
            
            
            
        }

        private void SetWeaponParametrs()
        {
            _weaponName = _smallArms.WeaponName;
            _damage = _smallArms.Damage;
            _maxAmmo = _smallArms.MaxAmmo;
            _fireRate = _weaponAnimInfo[_animController.IsShoot];
            _reloadTime = _weaponAnimInfo[_animController.Reload];
            _range = _smallArms.Range;
        }

        public int GetDamage()
        {
            _totalReceivedDamage += _damage;
            return _damage;
        }
        
        public float GetSpread()
        {
            float spread = 8;

            return spread;
        }
        

        public int DamageReceived() =>
            _totalReceivedDamage;

        public float GetRangeAttack()
        {
            return _range;
        }

        public Weapon GetActiveWeapon()
        {
            return _smallArms;
        }
        public void SetWeapon(Transform weaponTransform )
        {
            _weaponPrefab.transform.parent = weaponTransform;
        }

        private void SetAnimInfo()
        {
            foreach (KeyValuePair<int, float> info in _animController.GetAnimInfo())
            {
                _weaponAnimInfo.Add(info.Key, info.Value);
            }
        }
    }

    interface IWeapon
    {
         abstract int GetDamage();
         abstract float GetRangeAttack();
    }
    
//    Sniper Vector3(0.0289999992,-0.0839999989,0.0170000009)Vector3(347.422821,89.5062866,89.6258698)
//    shotgun Vector3(0.0179999992,-0.0500000007,-0.0189999994)Vector3(348.677673,89.4979019,89.6276169)
}