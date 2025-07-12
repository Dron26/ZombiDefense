using Infrastructure.BaseMonoCache.Code.MonoCache;
using Lean.Localization;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI.SettingsPanel
{
    public class LanguagePanel:MonoCache
    {
        [SerializeField] private Button _rusButton;
        [SerializeField] private Button _engButton;
        [SerializeField] private Button _trkButton;
        
        private LeanLocalization _localization;
        
        private string _rus = "Russian";
        private string _eng = "English"; 
        private string _trk = "Turkey";
        private string _RusLanguageCode = "ru";
        private string _EngLanguageCode = "en";
        private string _TrkLanguageCode = "tr";
        
        private void Awake()
        {
            _rusButton.onClick.AddListener(() => SetData(_rus));
            _engButton.onClick.AddListener(() => SetData(_eng));
            _trkButton.onClick.AddListener(() => SetData(_trk));
            
            _localization = FindObjectOfType<LeanLocalization>();
        }

        
        private void SetData(string language)
        {
            LeanLocalization.SetCurrentLanguageAll(language);
            if (language==_rus)
            {
                YG2.SwitchLanguage(_RusLanguageCode);
            }
            else if (language == _eng)
            {
                YG2.SwitchLanguage(_EngLanguageCode);
            }
            else if (language == _trk)
            {
                YG2.SwitchLanguage(_TrkLanguageCode);
                
            }
        }

        private void OnDestroy()
        {
            _rusButton.onClick.RemoveListener(() => SetData(_rus));
            _engButton.onClick.RemoveListener(() => SetData(_eng));
            _trkButton.onClick.RemoveListener(() => SetData(_trk));
        }
    }
}