using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;

namespace UI.HUD.StorePanel
{
    public class PricePanel: MonoCache
    {
        [SerializeField] private TMP_Text _priceText;
        private CharacterStore _characterStore;
        
        public void SetInfo()
        {
            _priceText.text = $"Price: ${_characterStore.SelectedCharacterSlot.Price.ToString()}";
        }

        public void Initialize(CharacterStore characterStore)
        {
            characterStore.OnUpdateBought += SetInfo;
            _characterStore = characterStore;
        }
    }
}