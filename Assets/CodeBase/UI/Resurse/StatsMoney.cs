using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;
using System.Collections;

namespace UI.Resurse
{
    [DisallowMultipleComponent]
    public class StatsMoney : MonoCache
    {
        public TMP_Text Text => _text;
        private TMP_Text _text;
        private Color _notEnoughColor = Color.white;
        private Color _defaultColor = Color.red;
        private float _blinkDuration = 2f;
        private float _blinkSpeed = 4f;

        public void Initialize(int money)
        {
            _text = GetComponentInChildren<TMP_Text>();
            _defaultColor = _text.color;
            SetMoney(money);
        }

        public void SetMoney(int money) => _text.text = "$" + money.ToString();


        public void ShowNotEnoughMoney()
        {
            StartCoroutine(BlinkColor());
        }

        private IEnumerator BlinkColor()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _blinkDuration)
            {

                float t = Mathf.PingPong(elapsedTime * _blinkSpeed, 1f);

                _text.color = Color.Lerp(_notEnoughColor, _defaultColor, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }


            _text.color = _defaultColor;
        }
    }
}