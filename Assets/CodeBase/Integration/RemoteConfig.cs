using System.Collections.Generic;
using YG;

public class RemoteConfig
{
    public static RemoteConfig Instance { get; private set; }

    public RemoteConfig()
    {
        Instance = this;
    }

    public bool DisableAds => TryGetBool("disableAds", out bool value) && value;

    public bool AllUpgradesUnlocked => TryGetBool("unlockAllUpgrades", out bool value) && value;
    public bool AllLocationsUnlocked => TryGetBool("unlockAllLocations", out bool value) && value;
    public bool GiveMoney => TryGetBool("giveMoney", out bool value) && value;

    public int GetMoneyAmount => TryGetInt("getMoneyAmount", out int value) ? value : 0;

    public float Difficulty => TryGetFloat("difficulty", out float value) ? value : 1f;

    public List<int> DailyRewardValues => TryParseIntList("dailyRewardValues");
    
    public bool IsSentAdditionalMetrics => TryGetBool("sentAdditionalMetrics", out bool value) && value;
    
    #region Try Get Methods

    private bool TryGetBool(string key, out bool result)
    {
        return YG2.TryGetFlagAsBool(key, out result);
    }

    private bool TryGetInt(string key, out int result)
    {
        //return YG2.TryGetFlagAsInt(key, out result);
        
        if (YG2.TryGetFlagAsInt(key, out result))
        {
            return true;
        }
        
        return false;
    }

    private bool TryGetFloat(string key, out float result)
    {
        return YG2.TryGetFlagAsFloat(key, out result);
    }

    #endregion

    private List<int> TryParseIntList(string key)
    {
        if (YG2.TryGetFlag(key, out string raw))
        {
            List<int> list = new List<int>();
            string[] tokens = raw.Split(new[] { ',', ';', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var token in tokens)
            {
                if (int.TryParse(token.Trim(), out int val))
                    list.Add(val);
            }
            return list;
        }

        return new List<int> {1000,2000,3000,4000,6000,10000,13000}; // Дефолтные награды
    }
}