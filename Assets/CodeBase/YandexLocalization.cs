using Infrastructure.BaseMonoCache.Code.MonoCache;
using Lean.Localization;
using UnityEngine;

public class YandexLocalization : MonoCache
{
    [SerializeField] private LeanLocalization _leanLocalization;

#if YANDEX_GAMES
    private IEnumerator Start()
    {
        DontDestroyOnLoad(_leanLocalization.gameObject);

        while (!YandexGamesSdk.IsInitialized)
        {
            yield return new WaitForSeconds(0.25f);

            if (YandexGamesSdk.IsInitialized)
                LoadLocalization();
        }
    }

    private void LoadLocalization()
    {
        switch (YandexGamesSdk.Environment.i18n.lang)
        {
            case "ru":
                _leanLocalization.SetCurrentLanguage("Russian");
                break;
            case "en":
                _leanLocalization.SetCurrentLanguage("English");
                break;
            case "tr":
                _leanLocalization.SetCurrentLanguage("Turkish");
                break;
            default:
                _leanLocalization.SetCurrentLanguage("English");
                break;
        }
    }
#endif

// #if VK_GAMES
//     private void Start()
//     {
//         DontDestroyOnLoad(_leanLocalization.gameObject);
//         _leanLocalization.SetCurrentLanguage("Russian");
//     }
// #endif
}