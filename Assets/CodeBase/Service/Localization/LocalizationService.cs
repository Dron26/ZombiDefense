using System;
using Unity.VisualScripting;

namespace CodeBase.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        public Icons.Language Language { get; private set; }

        public event Action LanguageChanged;

        public LocalizationService(Icons.Language language) =>
            ChangeLanguage(language);

        public void ChangeLanguage(Icons.Language language)
        {
            Language = language;
            LanguageChanged?.Invoke();
        }

        public string GetText(string russian, string turkish, string english)
        {
            switch (Language)
            {
                case Language.RU:
                    return russian;
                case Language.TR:
                    return turkish;
                default:
                    return english;
            }
        }
    }
}