using Data;
using Infrastructure.AIBattle;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
using Service.SaveLoad;
using UnityEngine;

namespace Infrastructure.Factories.FactoriesBox
{
    public class BoxFactory:MonoCache
    {
        public GameObject Create(BoxType type)
        {
            string path =AssetPaths.Boxes + type;
            GameObject newBox = Instantiate(Resources.Load<GameObject>(path));
           
            return newBox;
        }
    }
}