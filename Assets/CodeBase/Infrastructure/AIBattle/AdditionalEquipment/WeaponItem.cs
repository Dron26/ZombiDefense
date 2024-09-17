namespace Infrastructure.AIBattle.AdditionalEquipment
{
    public abstract class WeaponItem : BaseItem
    {
        public abstract int Damage { get; }
        public abstract float Range { get; }
    }
}