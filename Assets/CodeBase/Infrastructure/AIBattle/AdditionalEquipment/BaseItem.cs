using Infrastructure.BaseMonoCache.Code.MonoCache;

namespace Infrastructure.AIBattle.AdditionalEquipment
{
    public abstract class BaseItem : MonoCache
    {
        public abstract ItemType ItemType { get; }
        
        public abstract void Initialize(ItemData itemData);
    }
}