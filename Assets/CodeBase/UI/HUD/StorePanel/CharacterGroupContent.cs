using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace UI.HUD.StorePanel
{
    public class CharacterGroupContent : MonoCache
    {
        private GameObject _characterSlotPrefab;

        public void SetSlot(CharacterSlot characterSlot, CharacterStore store)
        {
            GameObject slotPrefab = Instantiate(_characterSlotPrefab, transform);
            CharacterSlot slot = slotPrefab.GetComponent<CharacterSlot>();
            slot.Initialize(characterSlot.Humanoid, store);
        }

        public void Initialize(GameObject characterSlotPrefab)
        {
            _characterSlotPrefab = characterSlotPrefab;
        }
    }
}