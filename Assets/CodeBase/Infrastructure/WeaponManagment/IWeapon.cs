namespace Infrastructure.WeaponManagment
{
    public interface IWeapon
    {
        abstract int GetDamage();
        abstract float GetRangeAttack();
    }
}