using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

[CreateAssetMenu(fileName = "WavesContainerData", menuName = "WavesContainerData")]
public class WavesContainerData : ScriptableObject
{
    public List<WaveData> GroupWaveData;
    public int LocationNumber;
}