using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.WeaponManagment
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
         
        private List<Weapon> _smallArms;
        private List<Weapon> _granads;

        public List<Weapon> SmallArms()
        {
            _smallArms = new() { _handgun, _rifle, _sniperRifle, _heavyMachinуeGun,_shootgun };
            return _smallArms;
        }
        
        public List<Weapon> Granads()
        {
            _granads = new() { _granade };
            return _granads;
        }
    }
}