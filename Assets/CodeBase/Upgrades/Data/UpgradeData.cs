using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string Id;
    public string Name;
    public string Description;
    public int Cost;
    public Sprite Icon;
    public UpgradeType Type;

    public float Value;  
    public int UnlockId; 
}