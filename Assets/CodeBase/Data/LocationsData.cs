using System;
using System.Collections.Generic;
using Data;
using UI.Levels;

[Serializable]
public class LocationsData
{
    public List<LocationData> LocationsDatas=new List<LocationData>();

    public LocationsData(List<LocationData> locationsDatas)
    {
        LocationsDatas=locationsDatas;
    }
}

public class LocationData
{
    public int Id;
    public int MaxEnemyOnLevel;
    public string Path;
    public bool IsTutorial;
    public bool IsLocked;
    public bool IsCompleted;
    public List<WaveData> WaveDatas;
}
