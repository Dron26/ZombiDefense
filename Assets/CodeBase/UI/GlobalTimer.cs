using System.Collections;
using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GlobalTimer : MonoCache
    {
        [SerializeField] private Button _buttonTime;
        [SerializeField] private GameObject _buttonPanel;
        [SerializeField] private Button _firstTime;
        [SerializeField] private Button _secondTime;
        [SerializeField] private Button _thirdTime;
        [SerializeField] private Button _fourthTime;
        [SerializeField] private List<TMP_Text> _texts;

        private bool _isPanelActive;
        private Color _colorDefault;
        private Coroutine _timerCoroutine; 

        private void Start()
        {
            _buttonTime.onClick.AddListener(ShowPanel);
            _firstTime.onClick.AddListener(() => SetTime(0.5f, 0,1));
            _secondTime.onClick.AddListener(() => SetTime(1f, 2,3));
            _thirdTime.onClick.AddListener(() => SetTime(1.5f, 4,5));
            _fourthTime.onClick.AddListener(() => SetTime(3f, 6,7));
            _colorDefault = _texts[0].color;
            _texts[2].color = Color.black;
            _texts[3].color = Color.black;
            _buttonPanel.SetActive(false);
        }

        private void SetTime(float timeScaleValue, int index, int timeScaleIndex)
        {
            foreach (var text in _texts)
            {
                text.color = _colorDefault;
            }
            _texts[index].color = Color.black;
            _texts[timeScaleIndex].color = Color.black;
            
            Time.timeScale = timeScaleValue;

            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
            }

            if (_isPanelActive)
            {
                _timerCoroutine = StartCoroutine(StartTimer());
            }
        }

        private void ShowPanel()
        {
            _isPanelActive = !_isPanelActive;
            _buttonPanel.SetActive(_isPanelActive);

            if (_isPanelActive)
            {
                _buttonTime.interactable = false;
                if (_timerCoroutine != null)
                {
                    StopCoroutine(_timerCoroutine);
                }
                _timerCoroutine = StartCoroutine(StartTimer());
            }
            else
            {
                if (_timerCoroutine != null)
                {
                    StopCoroutine(_timerCoroutine);
                    _timerCoroutine = null;
                }
                _buttonTime.interactable = true;
            }
        }

        private IEnumerator StartTimer()
        {
            yield return new WaitForSecondsRealtime(4f);

            _buttonTime.interactable = true;
            _buttonPanel.SetActive(false);
            _isPanelActive = false;
            _timerCoroutine = null; 
        }
    }
}