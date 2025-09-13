using Infrastructure.BaseMonoCache.Code.MonoCache;
using YG;
using Interface;
using UnityEngine;
using TMPro;

public class MissionUI : MonoCache
{
    [SerializeField] private TextMeshProUGUI _locationInfo;
    [SerializeField] private TextMeshProUGUI _context;
    [SerializeField] private TextMeshProUGUI _objective;
    [SerializeField] private TextMeshProUGUI _location;
    [SerializeField] private TextMeshProUGUI _tip;

    private LocationProgressData _currentLocation;

    protected override void OnEnabled()
    {
        YG2.onSwitchLang += OnSwitchLanguage;
    }

    protected override void  OnDisabled()
    {
        YG2.onSwitchLang -= OnSwitchLanguage;
    }

    public void DisplayMission(LocationProgressData data)
    {
        if (data == null)
        {
            Debug.LogError("LocationProgressData is null!");
            return;
        }

        _currentLocation = data;
        UpdateUI(YG2.lang);
    }

    private void OnSwitchLanguage(string lang)
    {
        if (_currentLocation != null)
        {
            UpdateUI(lang);
        }
    }

    private void UpdateUI(string lang)
    {
        _locationInfo.text = GetLocalizedText(_currentLocation.TitleRu, _currentLocation.TitleEn, _currentLocation.TitleTr, lang);
        _context.text = GetLocalizedText(_currentLocation.ContextRu, _currentLocation.ContextEn, _currentLocation.ContextTr, lang);
        _objective.text = GetLocalizedText(_currentLocation.ObjectiveRu, _currentLocation.ObjectiveEn, _currentLocation.ObjectiveTr, lang);
        _location.text = GetLocalizedText(_currentLocation.LocationRu, _currentLocation.LocationEn, _currentLocation.LocationTr, lang);
        _tip.text = GetLocalizedText(_currentLocation.TipRu, _currentLocation.TipEn, _currentLocation.TipTr, lang);
    }

    private string GetLocalizedText(string ru, string en, string tr, string currentLang)
    {
        switch (currentLang)
        {
            case "ru":
                return string.IsNullOrEmpty(ru) ? en : ru;
            case "en":
                return string.IsNullOrEmpty(en) ? ru : en;
            case "tr":
                return string.IsNullOrEmpty(tr) ? en : tr;
            default:
                Debug.LogWarning($"Unsupported language: {currentLang}. Falling back to English.");
                return en;
        }
    }
}