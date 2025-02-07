using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseAttackRangeUpgrade", menuName = "Upgrades/IncreaseAttackRange")]
public class IncreaseAttackRangeUpgrade : UpgradeEffect
{
    public float AttackRangeMultiplier; // Например, 1.5 = +50%

    public override void Apply()
    {
        //Увеличение дальности атаки:
    }
}