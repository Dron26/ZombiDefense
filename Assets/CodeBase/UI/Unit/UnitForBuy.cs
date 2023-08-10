using Infrastructure.BaseMonoCache.Code.MonoCache;
using UI.BuyAndMerge;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Unit
{
    [DisallowMultipleComponent]
    public class UnitForBuy : MonoCache
    {
        private CharacterSeller _characterSeller;
        private Button _button;
        public void Initialize(CharacterSeller characterSeller)
        {
            SetButton();
            _characterSeller = characterSeller;
            _button.onClick.AddListener(() => _characterSeller.BuyCharacter(gameObject));
        }

        private void OnDestroy()
        {
            if (_button != null)
                _button.onClick.RemoveListener(() => _characterSeller.BuyCharacter(gameObject));
            else
                SetButton();
            
            Destroy(GetComponent<Button>());
        }
        
        private void SetButton() => _button = GetComponent<Button>();
    }
}