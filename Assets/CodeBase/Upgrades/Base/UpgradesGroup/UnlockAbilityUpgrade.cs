using UnityEngine;

[CreateAssetMenu(fileName = "UnlockAbilityUpgrade", menuName = "Upgrades/UnlockAbility")]
public class UnlockAbilityUpgrade : UpgradeEffect
{
    public string AbilityId; 

    public override void Apply()
    {
        //Разблокировка новых умений:
    }
}