using System;
using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingsPanel
{
    public class LanguagePanel:MonoCache
    {
        [SerializeField] private Button _apply;
        [SerializeField] private Button _selectedLanguage;
        [SerializeField] private TMP_Text _selectedLanguageText;
        [SerializeField] private GameObject _languagesPanel;
        [SerializeField] private GameObject _languagePrefab;
        [SerializeField] private List<string> _currentLanguages = new();
        
        private LeanLocalization _localization;
        
        private string _currentLanguage=>_localization.CurrentLanguage;
        
        private string _RusLanguage = "Russian";
        private string _EngLanguage = "English"; 
        private string _TrkLanguage = "Turkey";
        private void Awake()
        {
            _localization = FindObjectOfType<LeanLocalization>();
            _selectedLanguage.onClick.AddListener(() => ShowPanel());
            _apply.onClick.AddListener(() => Apply());
            
            foreach (var language in _currentLanguages)
            {
                GameObject languagePrefab=Instantiate(_languagePrefab, _languagesPanel.transform);
                languagePrefab.GetComponent<TMP_Text>().text = language;
                languagePrefab.GetComponent<Button>().onClick.AddListener(() => SetData(language));
            }
            
            SetLanguageText();
        }

        private void Apply()
        {
            _languagesPanel.SetActive(false);
            _selectedLanguage.gameObject.SetActive(true);
        }

        private void ShowPanel()
        {
            _languagesPanel.SetActive(!_languagesPanel.activeSelf);
            _selectedLanguage.gameObject.SetActive(!_selectedLanguage.gameObject.activeSelf);
        }

        private void SetLanguageText( )
        {
            _selectedLanguageText.text = _currentLanguage;
        }
        
        private void SetData(string language)
        {
            LeanLocalization.SetCurrentLanguageAll(language);
            ShowPanel();
            SetLanguageText();
        }
        
        
    }
}