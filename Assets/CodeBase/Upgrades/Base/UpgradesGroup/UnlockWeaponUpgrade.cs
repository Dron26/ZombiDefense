using UnityEngine;

[CreateAssetMenu(fileName = "UnlockWeaponUpgrade", menuName = "Upgrades/UnlockWeapon")]
public class UnlockWeaponUpgrade : UpgradeEffect
{
    public string WeaponId; 

    public override void Apply()
    {
        //Разблокировка нового оружия:
    }
}