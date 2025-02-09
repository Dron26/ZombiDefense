using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public int Id;
    public string Name;
    public string Description;
    public int Cost;
    public Sprite Icon;
    public UpgradeType Type;
    public UpgradeGroup Group;

    public float Value;  
    public int UnlockId;
    public List<UpgradeEffect> UpgradeEffect;
    public bool Lock { get; set; }
    public Sprite IconUpgrade { get; set; }
    public Sprite IconLock { get; set; }
    public TextMeshProUGUI Info { get; set; }
    public TextMeshProUGUI Price { get; set; }
}