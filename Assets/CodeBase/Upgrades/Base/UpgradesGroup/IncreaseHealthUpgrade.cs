using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseHealthUpgrade", menuName = "Upgrades/IncreaseHealth")]
public class IncreaseHealthUpgrade : UpgradeEffect
{
    public float HealthMultiplier; // Например, 1.2 = +20%

    public override void Apply()
    {
        //Увеличение здоровья:
    }
}