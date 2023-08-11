namespace Infrastructure.Logic.WeaponManagment
{
    public interface IWeapon
    {
        abstract int GetDamage();
        abstract float GetRangeAttack();
    }
}