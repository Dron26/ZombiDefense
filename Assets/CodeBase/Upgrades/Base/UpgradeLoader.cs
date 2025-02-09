using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class UpgradeLoader : IUpgradeLoader
    {
        public List<UpgradeData> GetData()
        {
            List<UpgradeData> result = new List<UpgradeData>();
            UpgradeType[] types = (UpgradeType[])Enum.GetValues(typeof(UpgradeType));

            foreach (var type in types)
            {
                string path = $"UpgradeData/{type}"; // Путь к ресурсам
                UpgradeData data = Resources.Load<UpgradeData>(path);
                if (data != null)
                {
                    result.Add(data);
                }
            }
            
            return result;
        }
    }
}