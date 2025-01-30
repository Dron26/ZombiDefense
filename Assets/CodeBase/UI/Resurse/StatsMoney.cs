using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;

namespace UI.Resurse
{
    [DisallowMultipleComponent]
    public class StatsMoney : MonoCache
    {
        public TMP_Text Text => _text;
        private TMP_Text _text;
        
        public void Initialize(int money)
        {
            _text=GetComponentInChildren<TMP_Text>();
            SetMoney(money);
        }

        public void SetMoney(int money) => _text.text = "$"+money.ToString();
    }
}