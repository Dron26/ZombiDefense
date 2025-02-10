using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class UpgradeDataLogger : MonoBehaviour
{
    private const string DataFolderPath = "Assets/Resources/Prefab/UI/UpgradesMap/Data/"; // Папка с улучшениями

    [ContextMenu("Log All Upgrades")]
    private void LogAllUpgrades()
    {
        List<string> upgradeFolders = new List<string>
        {
            "AirStrike", "Box", "Damage", "DefencePoint", "GranadeDamage",
            "PriceUpdate", "Profit", "Range", "RestoreHealth", "RestoreHealthCost",
            "Size Squad", "SpecialCar", "StartCashLimit", "Turret", "Unit Level"
        };

        foreach (string folder in upgradeFolders)
        {
            string path = Path.Combine(DataFolderPath, folder);
            string[] guids = AssetDatabase.FindAssets("t:UpgradeData", new[] { path });

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                UpgradeData upgrade = AssetDatabase.LoadAssetAtPath<UpgradeData>(assetPath);

                if (upgrade != null)
                {
                    Debug.Log($"ID: {upgrade.Id}\n" +
                              $"Name: {upgrade.Name}\n" +
                              $"Description: {upgrade.Description}\n" +
                              $"Cost: {upgrade.Cost}\n" +
                              $"Type: {upgrade.Type}\n" +
                              $"GroupType: {upgrade._groupType}\n" +
                              $"UnlockId: {upgrade.UnlockId}\n" +
                              $"UpgradesValue: {string.Join(", ", upgrade.UpgradesValue)}\n\n\n");
                }
            }
        }
    }
}