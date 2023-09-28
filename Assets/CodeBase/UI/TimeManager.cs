using System.Collections;
using System.Globalization;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TimeManager : MonoCache
    {
        [SerializeField] private Button _buttonTime;
        [SerializeField] private GameObject _buttonPanel;
        [SerializeField] private Button _firstTime;
        [SerializeField] private Button _secondTime;
        [SerializeField] private Button _thirdTime;
        [SerializeField] private Button _fourthTime;
        
        private TMP_Text[] _texts;
        private TMP_Text[] _selectedTexts;
        private bool _isPanelActive;
        private Color _colorDefault;
        private float _timePause = 0;
        private float _timeNormal = 1;
        
        public void Start()
        {
            _buttonTime.onClick.AddListener(ShowPanel);
            _firstTime.onClick.AddListener(() => SetTime(_firstTime.gameObject));
            _secondTime.onClick.AddListener(() => SetTime(_secondTime.gameObject));
            _thirdTime.onClick.AddListener(() => SetTime(_thirdTime.gameObject));
            _fourthTime.onClick.AddListener(() => SetTime(_fourthTime.gameObject));
            _buttonPanel.SetActive(false);
        }

        private void SetTime(GameObject selectedTime)
        {
            _texts = selectedTime.GetComponentsInChildren<TextMeshProUGUI>();

            if (float.TryParse(_texts[1].text, NumberStyles.Float, CultureInfo.InvariantCulture,
                    out float timeScaleValue))
            {
                Time.timeScale = timeScaleValue;
            }

            SetColorText();
        }

        private void SetColorText()
        {
            if (_selectedTexts != null && _selectedTexts != _texts)
            {
                _colorDefault = _texts[1].color;

                foreach (var item in _selectedTexts)
                {
                    item.color = _colorDefault;
                }
            }

            for (int i = 0; i < _texts.Length; i++)
            {
                _texts[i].color = Color.black;
            }

            _selectedTexts = _texts;
        }

        private void ShowPanel()
        {
            _isPanelActive = !_isPanelActive;
            _buttonPanel.SetActive(_isPanelActive);
            
            if (_isPanelActive)
            {
                StartCoroutine(StartTimer());
            }
            else
            {
                StartCoroutine(StartTimer());
            }
        }

        private IEnumerator StartTimer()
        {
            yield return new WaitForSecondsRealtime(3);
            
            _buttonPanel.SetActive(false);
            _isPanelActive = !_isPanelActive;
        }

        public void SetPaused(bool isActive)
        {
            
            if (isActive)
            {
                _timeNormal=Time.timeScale;
                Time.timeScale = _timePause;
            }
            else
            {
                Time.timeScale = _timeNormal;
            }
            
        }
    }
}