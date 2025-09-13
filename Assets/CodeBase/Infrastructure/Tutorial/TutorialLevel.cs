using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.Inits;
using Infrastructure.Logic.WaveManagment;
using Infrastructure.Points;
using Interface;
using Lean.Localization;
using Services;
using Services.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Infrastructure.Tutorial
{
    public class TutorialLevel : MonoCache
    {
        [SerializeField] private List<Image> _images;
        [SerializeField] private List<Image> _arrows;
        [SerializeField] private List<Button> _buttons;
       [SerializeField] private TextMeshProUGUI _tutorialText;
        [SerializeField] private Image _dimm;
        [SerializeField] private Button _back;
        [SerializeField] private GameObject _panel;
        private Image _currentImage;
        private int _currentImageIndex = 0;
        private int _currentDialogIndex = 0;
        private bool _keyPressed = false;
        private Coroutine _tutorialCoroutine;
        private Coroutine _brightnessCoroutine;
        private Color _currentColor;
        public Action OnEndTutorial;

        // Локализованные тексты для туториала
        private readonly string[] _dialogsRu = {
            "Здравия желаю, Капитан! Зомби атакуют аванпост:Вам желательно пройти обучение!",
            "Будь внимателен, я подсвечу элементы управления. Нажимай любую клавишу, чтобы продолжить.",
            "Это меню магазина. Покупай бойцов и оборудование для защиты аванпоста.",
            "Кнопка повышения уровня позиции. Улучшает навыки бойцов на ней.",
            "Тут можешь купить доп снаряжение и медикаменты.",
            "Скрывает нижнюю панель управления для удобства.",
            "Управляй скоростью игры и бойцами здесь.",
            "Показывает количество врагов на локации.",
            "Твой кредитный баланс для покупок.",
            "Выход в меню игры.",
            "Теперь ты готов защищать аванпост! Начинай операцию!"
        };

        private readonly string[] _dialogsEn = {
            "I wish you good health, Captain! Zombies are attacking the outpost:It is advisable for you to complete the training!",
            "Be careful, I'll highlight the controls. Press any key to continue.",
            "This is the store menu. Buy fighters and equipment to defend the outpost.",
            "Position upgrade button. Improves the skills of the fighters on it.",
            "Here you can buy additional equipment and medicines.",
            "Hides the lower control panel for convenience.",
            "Control the speed of the game and the fighters here.",
            "Shows the number of enemies in the location.",
            "Your credit balance for purchases.",
            "Exit the game menu.",
            "Now you're ready to defend the outpost! Start the operation!"
        };

        private readonly string[] _dialogsTr = {
            "lamlar dilerim, Kaptan! Zombiler karakola saldırıyor: Eğitim almanız tavsiye edilir!",
            "Dikkatli ol, kontrolleri aydınlatacağım. Devam etmek için herhangi bir tuşa bas.",
            "Bu mağazanın menüsü. Karakolu korumak için savaşçılar ve ekipman satın alın.",
            "Konum seviyesini yükseltme düğmesi. Savaşçıların üzerindeki becerilerini geliştirir.",
            "Burada ek ekipman ve ilaç satın alabilirsiniz.",
            "Kolaylık sağlamak için alt kontrol panelini gizler.",
            "Oyunun hızını ve buradaki savaşçıları kontrol edin.",
            "Konumdaki düşman sayısını gösterir.",
            "Satın alımlar için kredi bakiyeniz.",
            "Oyun menüsüne çıkış.",
            "Artık karakolu savunmaya hazırsın! Ameliyata başla!"
        };

        protected override void OnEnabled()
        {
            YG2.onSwitchLang += OnSwitchLanguage;
            _back.onClick.AddListener(EndTutorial);
        }

        protected override void OnDisabled()
        {
            YG2.onSwitchLang -= OnSwitchLanguage;
            _back.onClick.RemoveListener(EndTutorial);
        }
        

        public void Start()
        {
            if (!AllServices.Container.Single<ILocationHandler>().GetCurrentLocationData().IsTutorial)
                return;

            if (AllServices.Container.Single<IAchievementsHandler>().IsTutorialEnded())
                return;
            
            _panel.SetActive(true);
            _dimm.enabled = true;
            _currentImageIndex = 0;
            _currentDialogIndex = 0;
            _tutorialCoroutine = StartCoroutine(RunTutorial());
        }

        private IEnumerator RunTutorial()
        {
            yield return new WaitForSeconds(1f);

            while (_currentDialogIndex < _dialogsRu.Length)
            {
                _keyPressed = false;
                UpdateText(YG2.lang);

                if (_currentDialogIndex >= 2 && _currentImageIndex < _buttons.Count+1)
                {
                    ChangeImageAndBrightness();
                }

                yield return WaitForKeyPress();
                _currentDialogIndex++;
            }

            EndTutorial();
        }

        private void UpdateText(string lang)
        {
            _tutorialText.text = GetLocalizedText(lang);
        }

        private string GetLocalizedText(string lang)
        {
            switch (lang)
            {
                case "ru":
                    return _dialogsRu[_currentDialogIndex];
                case "en":
                    return _dialogsEn[_currentDialogIndex];
                case "tr":
                    return _dialogsTr[_currentDialogIndex];
                default:
                    Debug.LogWarning($"Unsupported language: {lang}. Falling back to English.");
                    return _dialogsEn[_currentDialogIndex];
            }
        }

        private IEnumerator WaitForKeyPress()
        {
            while (!_keyPressed)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    EndTutorial();
                    yield break;
                }
                else if (Input.anyKeyDown)
                {
                    _keyPressed = true;
                    if (_currentDialogIndex >= 2 && _currentImageIndex < _buttons.Count)
                    {
                        _arrows[_currentImageIndex].gameObject.SetActive(false);
                        if (_brightnessCoroutine != null)
                        {
                            StopCoroutine(_brightnessCoroutine);
                        }
                        ChangeImageBrightness(1f);
                        _currentImageIndex++;
                    }
                }

                yield return null;
            }
        }

        private void EndTutorial()
        {
            if (_brightnessCoroutine != null)
            {
                StopCoroutine(_brightnessCoroutine);
            }
            if (_tutorialCoroutine != null)
            {
                StopCoroutine(_tutorialCoroutine);
            }
            
            //_blockDimm.enabled = false;
            _dimm.enabled = false;
            AllServices.Container.Single<IAchievementsHandler>().EndTutorial();
            _panel.SetActive(false);
            gameObject.SetActive(false);
        }

        private void ChangeImageAndBrightness()
        {
            _currentImage = _images[_currentImageIndex];
            _brightnessCoroutine = StartCoroutine(ChangeImageBrightness());
        }

        private IEnumerator ChangeImageBrightness()
        {
            bool increasingBrightness = true;
            float currentBrightness = 1f;
            float brightnessChangeStep = 0.03f;
            float brightnessChangeInterval = 0.01f;

            _currentColor = _currentImage.color;
            _arrows[_currentImageIndex].gameObject.SetActive(true);

            while (!_keyPressed)
            {
                if (increasingBrightness && currentBrightness >= 1f)
                {
                    increasingBrightness = false;
                }
                else if (!increasingBrightness && currentBrightness <= 0f)
                {
                    increasingBrightness = true;
                }

                ChangeImageBrightness(currentBrightness);

                currentBrightness += increasingBrightness ? brightnessChangeStep : -brightnessChangeStep;
                yield return new WaitForSeconds(brightnessChangeInterval);
            }

            ChangeImageBrightness(1f);
        }

        private void ChangeImageBrightness(float brightness)
        {
            _currentImage.color = new Color(brightness, brightness, brightness, _currentColor.a);
        }

        private void OnSwitchLanguage(string lang)
        {
            UpdateText(lang);
        }
    }
}