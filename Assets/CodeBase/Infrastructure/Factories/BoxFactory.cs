using Infrastructure.AIBattle;
using Infrastructure.AIBattle.AdditionalEquipment;
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
        public AdditionalBox Create(BoxType type)
        {
            string path =AssetPaths.Boxes + type;
            GameObject newBox = Instantiate(Resources.Load<GameObject>(path));
            AdditionalBox box = newBox.GetComponent<AdditionalBox>();
             path =AssetPaths.BoxesData + type;
             
             box.Initialize(Resources.Load<BoxData>(path));
            return box;
        }
    }
}