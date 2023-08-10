using System;
using System.Collections;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UI.Empty;
using UI.SceneSetArmy;
using UI.SceneSetArmy.Slots;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Unit
{
    [DisallowMultipleComponent]
    public class SliderUnit : MonoCache, IPointerUpHandler, IPointerDownHandler
    {
        public int SliderValue => Convert.ToInt32(Math.Round(_slider.value));
        public int Index => _index;
        public UnityAction<GameObject> ChangeValue;
        public UnityAction<int, int> SelectSlider;

        private PlayerSlotsInitializer _player;
        private Slider _slider;
        private InfoFocusBlue _focusBlue;
        private InfoFocusGold _focusGold;
        private ShowerSliderValue _showerSlider;
        private int _index;
        private bool _isSendValueWork;

        public void Initialize(int numder)
        {
            _slider = GetComponent<Slider>();
            _player = transform.parent.GetComponentInParent<PlayerSlotsInitializer>();
            _index = numder;
            GameObject parent = transform.parent.gameObject;
            _showerSlider = parent.GetComponentInChildren<ShowerSliderValue>();
            _focusBlue = _showerSlider.GetComponentInChildren<InfoFocusBlue>();
            _focusGold = _showerSlider.GetComponentInChildren<InfoFocusGold>();
            _focusGold.gameObject.SetActive(false);
            _slider.value = _slider.minValue = 1;
        }

        public void SetMaxVolume(int value) => _slider.maxValue = value;

        public void SetMinVolume() => _slider.value = _slider.minValue;

        public void OnPointerUp(PointerEventData eventData)
        {
            _focusBlue.gameObject.SetActive(true);
            _focusGold.gameObject.SetActive(false);
            _isSendValueWork = false;
            ChangeValue?.Invoke(gameObject);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isSendValueWork = true;
            _focusBlue.gameObject.SetActive(false);
            _focusGold.gameObject.SetActive(true);
            StartCoroutine(SendValue(eventData));
        }

        private IEnumerator SendValue(PointerEventData eventData)
        {
            float time = 0.2f;
            while (_isSendValueWork)
            {
                int number = Convert.ToInt32(Math.Round(_slider.value));
                SelectSlider?.Invoke(number, Index);
                yield return new WaitForSeconds(time);
            }

            yield break;
        }
    }
}