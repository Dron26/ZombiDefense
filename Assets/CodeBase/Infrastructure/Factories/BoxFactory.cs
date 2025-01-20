using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Infrastructure.Factories
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