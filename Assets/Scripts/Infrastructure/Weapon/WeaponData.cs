using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Weapon
{
    [CreateAssetMenu(fileName = "New PrefabWeapon Data", menuName = "Weapons/PrefabWeapon Data")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private List<Weapon> _weapons;
        
        public List<Weapon> Weapons=> _weapons;
    }
}