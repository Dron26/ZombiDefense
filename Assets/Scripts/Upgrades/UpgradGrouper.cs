using System;
using System.Collections.Generic;
using Humanoids;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.WeaponManagment;
using Service.SaveLoadService;
using TextMesh_Pro.Examples___Extras.Scripts;
using UI.HUD.StorePanel;
using UnityEngine;
using Upgrades;

namespace Upgrades
{
    public class UpgradGrouper : MonoCache
    {
        [SerializeField] private  List<UpgradeData> allUpgradeDatas = new();
        [SerializeField]private UpgradeSlot _upgradeSlotPrefab;
        
        Array humanoidTypeValues = Enum.GetValues(typeof(HumanoidType));
        private List<UpgradeGroup> _upgradeGroups = new();
        private List<int> _currentLevel = new();
        private List<List<UpgradeData>> _upgradesDataTypes = new();

        public Action<UpgradeData,int,int> OnBuyUpgrade;
        
        private SelectedData _selectedData;
        
        public void Initialize(SaveLoad _saveLoad, CharacterStore _characterStore )
        {
            _characterStore.OnUpdateBought+=OnUpdateBought;
            FillUpgrades();
            FillUpgradeGroup();
        }

        private void OnUpdateBought()
        {
            _upgradeGroups[_selectedData.UpgradesGroupIndex].SetCurrentLevel(_selectedData.CurrentLevel);
        }

        private void FillUpgrades()
        {
            for (int i = 0; i < humanoidTypeValues.Length; ++i)
            {
                HumanoidType humanoidType = (HumanoidType)humanoidTypeValues.GetValue(i);
                int value = (int)humanoidType;

                _upgradesDataTypes.Add(new List<UpgradeData>());

                foreach (UpgradeData upgradeData in allUpgradeDatas)
                {
                    if ((int)upgradeData.HumanoidType == value)
                    {
                        _upgradesDataTypes[i].Add(upgradeData);
                    }
                }
            }

        }

        private void FillUpgradeGroup()
        {
            _upgradeGroups.AddRange(GetComponentsInChildren<UpgradeGroup>());

            for(int i = 0; i < _upgradeGroups.Count; ++i){
                
                _upgradeGroups[i].Initialize(_upgradesDataTypes[i], _upgradeSlotPrefab,_currentLevel[i]);
                _upgradeGroups[i].OnBuyUpgrade +=(upgradeData)=> TryBuyUpgrade(upgradeData,i);
                
            }
        }

        private void TryBuyUpgrade(UpgradeData upgradeData,  int index)
        {
            OnBuyUpgrade?.Invoke(upgradeData,upgradeData.Price,_currentLevel[index]); 
            _selectedData = new SelectedData(index,upgradeData,_currentLevel[index]);
        }
    }
}

class SelectedData
{
    private int _upgradeGroupIndex;
    private UpgradeData _upgradeData;
    private int _currentLevel;
    
    public int UpgradesGroupIndex => _upgradeGroupIndex;
    public int CurrentLevel => _currentLevel;

    public SelectedData(int upgradeGroupIndex, UpgradeData upgradeData, int currentLevel)
    {
        _upgradeGroupIndex = upgradeGroupIndex;
        _upgradeData = upgradeData;
        _currentLevel = currentLevel;
    }
}

//private Dictionary<string, List<UpgradeData>> upgradeDatas = new Dictionary<string, List<UpgradeData>>();
// List<string> nameCharacters = new List<string>();
//List<UpgradeInfo> upgrades = new List<UpgradeInfo>();
// private int healthIncreasePercentage = 30;
// private int damageIncreasePercentage = 20;
// private List<Humanoid> _humanids = new (); 
// private Dictionary<int, UpgradeInfo> upgrades = new ();
// private int _maxLevel = 3;
// private SaveLoad _saveLoad;
//         
// public void Initialize(SaveLoad saveLoad)
// {
//     _saveLoad = saveLoad;
//     _humanids=_saveLoad.GetActiveHumanoids();
//
//     InitializeHumanoidDatas();
// }
//
// private void  InitializeHumanoidDatas()
// {
//     foreach (Humanoid humanoid in _humanids)
//     {
//         upgrades.Add(humanoid.ID,new UpgradeInfo
//         {
//             Health = humanoid.GetHealth(),
//             Damage = humanoid.GetWeaponController().GetDamage(),
//             Level = humanoid.GetLevel()
//         });
//     }
// }
//
// private void UpgradeHumanoidData(Humanoid humanoid)
// {
//     int iD= humanoid.ID;
//     upgrades[iD].Level++;
//     int healthIncreaseAmount =(healthIncreasePercentage / 100) * upgrades[iD].Health;
//     upgrades[iD].Health += healthIncreaseAmount;
//     int damageIncreaseAmount =(damageIncreasePercentage / 100) * upgrades[iD].Damage;
//     upgrades[iD].Damage += damageIncreaseAmount;
//
//     humanoid.SetUpgrade(upgrades[iD]);
// }
// }
//
// Type characterNamesType = typeof(Infrastructure.Constants.CharacterNames);
// FieldInfo[] fields = characterNamesType.GetFields(BindingFlags.Public | BindingFlags.Static);
// foreach (FieldInfo field in fields)
// {
//     if (field.FieldType == typeof(string))
//     {
//         string fieldName = (string)field.GetValue(null);
//         nameCharacters.Add(fieldName);
//     }
// }