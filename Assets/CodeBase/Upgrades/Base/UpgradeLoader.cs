using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.AssetManagement;
using UnityEngine;

namespace Services
{
    public class UpgradeLoader : IUpgradeLoader
    {
        private static List<UpgradeData> _cache = new();

        public List<UpgradeData> GetData()
        {
            if (_cache.Count > 0) return _cache;
            
            var allUpgrades = Resources.LoadAll<UpgradeData>(AssetPaths.UpgradesData);

            foreach (var data in allUpgrades)
            {
                _cache.Add(data);
            }

            return _cache;
        }
    }
}