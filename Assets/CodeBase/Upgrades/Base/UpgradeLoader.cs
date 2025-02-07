using System;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.AssetManagement;
using UnityEngine;

namespace Services
{
    public class UpgradeLoader:IUpgradeLoader
    {
        public List<UpgradeData> GetData()
        {
            List<UpgradeData> result = new List<UpgradeData>();
            UpgradeType[] types = (UpgradeType[])Enum.GetValues(typeof(UpgradeType));

            for (int i = 0; i < types.Length; i++)
            {
                string path = AssetPaths.UpgradeData + types[i];
                UpgradeData data = Resources.Load<UpgradeData>(path);
                result.Add(data);
            }
            
            return result;
        }
        
    }
}