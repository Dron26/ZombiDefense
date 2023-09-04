using System;
using System.Collections.Generic;
using Data;

[Serializable]
public class LevelData
{
    
    public List<WaveData> WaveDatas;
    public int Number;
    public string Path;
    public bool IsTutorial;
}