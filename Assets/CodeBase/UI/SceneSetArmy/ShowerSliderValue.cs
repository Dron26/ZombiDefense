using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SceneSetArmy
{
    [DisallowMultipleComponent]
    public class ShowerSliderValue : MonoCache
    {
        [SerializeField] private Slider _slider;

        public TMP_Text Text => _text;
        
        private TMP_Text _text;
        
        private void Awake() => _text=GetComponentInChildren<TMP_Text>();

        public void ShowValue() => _text.text = Mathf.Round(_slider.value).ToString();
        
        public void SetHeroesValue(int value) => _text.text = Mathf.Round(value).ToString();
    }
}
