using Data.Upgrades;
using Infrastructure.Location;
using UI.Buttons;

public interface IWeaponController
{
    public int Damage { get; set; }
    public void Initialize();
    public float GetSpread();
    public void SetUpgrade(UpgradeData upgradeData, int level);
    public void UIInitialize();
    public void SetPoint(WorkPoint workPoint);
    public void SetSelected(bool isSelected);
    public void SetAdditionalWeaponButton(AdditionalWeaponButton additionalWeaponButton);
}