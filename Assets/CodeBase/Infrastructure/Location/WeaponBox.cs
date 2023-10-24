using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;
using UnityEngine.Serialization;

namespace Infrastructure.Location
{
    public class WeaponBox:MonoCache
    {
        [SerializeField] private List<Granade> _granades;
        [SerializeField] private List<Weapon> _weapons;

        public List<Granade> GetGranades()
        {
            return _granades;
        }
        
        public List<Weapon> GetWeapons()
        {
            return _weapons;
        }

        public void SetData(List<Granade> granades,List<Weapon> weapons)
        {
            _granades=granades;
            _weapons=weapons;
        }
    }
}