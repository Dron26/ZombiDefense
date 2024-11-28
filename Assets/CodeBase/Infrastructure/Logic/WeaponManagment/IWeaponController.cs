using Characters.Humanoids.AbstractLevel;
using Infrastructure.Location;

public interface IWeaponController
{
    public void Initialize(CharacterData data);
    public void SetPoint(WorkPoint workPoint);
    public void SetSelected(bool isSelected);
}