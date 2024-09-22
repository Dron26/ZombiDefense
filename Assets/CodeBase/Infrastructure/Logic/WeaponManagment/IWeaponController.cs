using Data.Upgrades;
using Infrastructure.Location;
using UI.Buttons;

public interface IWeaponController
{
    public void Initialize();
    public void SetPoint(WorkPoint workPoint);
    public void SetSelected(bool isSelected);
}