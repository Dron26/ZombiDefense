using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;

namespace Upgrades
{
    public class UpgradeManager : MonoCache
    {
        //private Dictionary<string, List<UpgradeData>> upgradeDatas = new Dictionary<string, List<UpgradeData>>();
        // List<string> nameCharacters = new List<string>();
        //List<UpgradeInfo> upgrades = new List<UpgradeInfo>();
        private int healthIncreasePercentage = 30;
        private int damageIncreasePercentage = 20;
        private List<Humanoid> _humanids = new (); 
        private Dictionary<int, UpgradeInfo> upgrades = new ();
        private int _maxLevel = 3;
        private SaveLoad _saveLoad;
        
        public void Initialize(SaveLoad saveLoad)
        {
            _saveLoad = saveLoad;
            _humanids=_saveLoad.GetActiveHumanoids();

            InitializeHumanoidDatas();
        }

        private void  InitializeHumanoidDatas()
        {
            foreach (Humanoid humanoid in _humanids)
            {
                upgrades.Add(humanoid.ID,new UpgradeInfo
                {
                    Health = humanoid.GetHealth(),
                    Damage = humanoid.GetWeaponController().GetDamage(),
                    Level = humanoid.GetLevel()
                });
            }
        }

        private void UpgradeHumanoidData(Humanoid humanoid)
        {
            int iD= humanoid.ID;
            upgrades[iD].Level++;
            int healthIncreaseAmount =(healthIncreasePercentage / 100) * upgrades[iD].Health;
            upgrades[iD].Health += healthIncreaseAmount;
            int damageIncreaseAmount =(damageIncreasePercentage / 100) * upgrades[iD].Damage;
            upgrades[iD].Damage += damageIncreaseAmount;

            humanoid.SetUpgrade(upgrades[iD]);
        }
    }
}


public class UpgradeInfo
{
    public int Health ;
    public int Damage ;
    public int Level;
}

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