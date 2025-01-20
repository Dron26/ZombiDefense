using System;
using Data.Settings.Language;

namespace Services.Localization
{
    public interface ILocalizationService : IService
    {
        Language Language { get; }

        public event Action LanguageChanged;

        string GetText(string russian, string turkish, string english);
        void ChangeLanguage(Language language);
    }
}