using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "GameParameters", menuName = "GameParametersData")]
    public class GameParametersData:ScriptableObject
    {
        public BoxType CurrentWeaponBoxType;
    }
}