using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.WeaponManagment
{
    [CreateAssetMenu(fileName = "New PrefabWeapon Data", menuName = "SmallArms/PrefabWeapon Data")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private Weapon _pistol;
        [SerializeField] private Weapon _rifle;
        [SerializeField] private Weapon _sniperRifle;
        [SerializeField] private Weapon _heavyMachinуeGun;
        [SerializeField] private Weapon _shotGun;
        
        [SerializeField] private Weapon _granade;
         
        private List<Weapon> _smallArms;
         private List<Weapon> _granads;

        public List<Weapon> SmallArms()
        {
            _smallArms = new() { _pistol, _rifle, _sniperRifle, _heavyMachinуeGun,_shotGun };
            return _smallArms;
        }
        
        public List<Weapon> Granads()
        {
            _granads = new() { _granade };
            return _granads;
        }
    }
}