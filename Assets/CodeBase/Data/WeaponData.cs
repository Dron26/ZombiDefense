using System.Collections.Generic;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "New PrefabWeapon Data", menuName = "SmallArms/PrefabWeapon Data")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private Weapon _handgun;
        [SerializeField] private Weapon _rifle;
        [SerializeField] private Weapon _sniperRifle;
        [SerializeField] private Weapon _heavyMachinуeGun;
        [SerializeField] private Weapon _shootgun;
        [SerializeField] private Weapon _granade;
        public bool IsGranade => _isGranade;
        private bool _isGranade=false;
        
        private List<Weapon> _smallArms=new();
        private List<Weapon> _granads=new();

        private void Awake()
        {
            if (_granads.Count != 0)
            {
                _isGranade = true;
            }
        } 

        public List<Weapon> GetSmallArms()
        {
            _smallArms = new() { _handgun, _rifle, _sniperRifle, _heavyMachinуeGun,_shootgun };
            return _smallArms;
        }
        
        public List<Weapon> GetGranads()
        {
            _granads = new() { _granade };
            return _granads;
        }
    }
}