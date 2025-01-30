using Infrastructure.Logic.WeaponManagment;

namespace Enemies
{
    public interface IDamageable
    {
        public void ApplyDamage(float damage, ItemType itemType);
    }
}