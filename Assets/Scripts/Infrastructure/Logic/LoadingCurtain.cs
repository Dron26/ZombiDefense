using System;
using System.Collections;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Infrastructure.Logic
{
    [DisallowMultipleComponent]
    public class LoadingCurtain : MonoCache
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _sliderText;
        public event Action OnFinishedShow;
   
        public CanvasGroup _canvasGroup;
    
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.interactable=false;
        }

        public void Hide(bool isMain) => StartCoroutine(Delay(isMain));
    
    
        private IEnumerator Delay(bool isMain)
        {
            _canvasGroup.alpha = 1;
            
            while (_slider.value < 100)
            {
                var delay = Random.Range(0.02f, 0.05f);
                _slider.value += 4.1f;
                _sliderText.text = ($"...{Math.Round(_slider.value, 1)}%");
                yield return new WaitForSeconds(delay);
            }

            DisableCurtain(isMain);
        }
    
        private void DisableCurtain(bool isMain)
        {
            StopCoroutine(Delay(isMain));
            _slider.value = 0;
            _canvasGroup.alpha = 0;
            if (isMain)
            {
                OnFinishedShow.Invoke();
            }
        }
        // private IEnumerator Delay()
        // {
        //   while (_canvasGroup.alpha > 0)
        //   {
        //     _canvasGroup.alpha -= 0.03f;
        //     yield return new WaitForSeconds(0.03f);
        //   }
        //   
        //   gameObject.SetActive(false);
        // }
    }
}