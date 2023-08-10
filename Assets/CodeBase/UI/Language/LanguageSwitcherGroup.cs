using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Lean.Localization;
using UnityEngine;

namespace UI.Language
{
    public class LanguageSwitcherGroup:MonoCache
    {
        [SerializeField] private LeanLocalization _leanLocalization;
        private List<string> _language = new();
        private List<ButtonSwitchLanguage> _buttons = new();

        private void Awake()
        {
            FillLanguages();
            InitializeLanguageButtons();
        }

        protected override void OnEnabled()
        {
            _leanLocalization.Changelanguage += SetActiveIconCheck;
        }

        private void InitializeLanguageButtons()
        {
            foreach (Transform transform in transform)
            {
                ButtonSwitchLanguage _buttonSwitch=transform.GetComponent<ButtonSwitchLanguage>();
                _buttonSwitch.SetIconCheck(false);
                _buttons.Add(_buttonSwitch);
            }

            for (int i = 0; i < _buttons.Count; i++)
            {
                _buttons[i].SetName(_language[i]);
            }
        }

        private void Start() => SetActiveIconCheck();

        public void SetActiveIconCheck()
        {
            foreach (var button in _buttons)
            {
                button.SetIconCheck(button.Name == _leanLocalization.CurrentLanguage);
            }
        }
        
        private void FillLanguages()
        {
            _language.Add("English");
            _language.Add("Russian");
            _language.Add("Turkey");
        }

        protected override void OnDisabled()
        {
            _leanLocalization.Changelanguage-=SetActiveIconCheck;
        }
    }
}