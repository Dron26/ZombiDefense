using System.Collections.Generic;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "BoxesPrice", menuName = "BoxesPrice")]
    public class BoxesPrice : ScriptableObject
    {
        public Dictionary<BoxType, int> BoxPrices = new Dictionary<BoxType, int>();
    }
}