using System;
using System.Collections;
using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.Inits;
using Infrastructure.Points;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Tutorial
{
    public class TutorialLevel : MonoCache
    {
        [SerializeField] private TMP_Text tutorialText;
        [SerializeField] private List<Image> _images;
        [SerializeField] private Humanoid _humanoid;
        [SerializeField] private WorkPoint _firstPoint;
        [SerializeField] private WorkPoint _secondPoint;
        
        private Image _currentImage;
        private LeanLocalizedTextMeshProUGUI _localizedTextMesh;
        private int currentDialogIndex = 0;
        private List<string> _dialogTexts;
        private Color _currentColor;
        private Coroutine tutorialCoroutine;
        private int _numberForStartChangeColor=2;
        private bool _keyPressed = false;
        private int _currentImageIndex = 0;
        public Action OnEndTutorial;
private bool _isCharacterCreated = false;
        private PlayerCharacterInitializer _characterInitializer;
        private EnemyCharacterInitializer _enemyCharacterInitializer;
private int numberForCreatedCharacter=8;
private bool _isChangedColor = false;
private MovePointController _movePointController;


        private void Start()
        {
            _localizedTextMesh = tutorialText.GetComponent<LeanLocalizedTextMeshProUGUI>();
            _characterInitializer=GetComponentInChildren<PlayerCharacterInitializer>();
            _enemyCharacterInitializer=GetComponentInChildren<EnemyCharacterInitializer>();
        }

        private void HandleEscapeKey()
        {
            EndTutorial();
        }

        public void Initialize(SceneInitializer sceneInitializer)
        {
            _dialogTexts = new List<string>();
            _movePointController = sceneInitializer.GetMovePointController();
            
            foreach (TutorialDialogKey dialog in Enum.GetValues(typeof(TutorialDialogKey)))
            {
                _dialogTexts.Add(dialog.ToString());
            }

            tutorialCoroutine = StartCoroutine(RunTutorial());
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public IEnumerator RunTutorial()
        {
            yield return  new WaitForSeconds(1f);
            
            foreach (string dialogText in _dialogTexts)
            {
                _keyPressed = false;
                _localizedTextMesh.TranslationName = dialogText;

                if (currentDialogIndex>=_numberForStartChangeColor&&_currentImageIndex<_images.Count)
                {
                    _isChangedColor = true;
                    ChangeImageAndBrightness();
                }else
                {
                    _isChangedColor = false;
                }

                if (currentDialogIndex==numberForCreatedCharacter)
                {
                    SetCharacters();
                }
                if (currentDialogIndex==numberForCreatedCharacter+1)
                {
                    MoveCharacters(_firstPoint);
                }
                if (currentDialogIndex==numberForCreatedCharacter+2)
                {
                    MoveCharacters(_secondPoint);
                }
               
                yield return WaitForKeyPress();

                currentDialogIndex++;
            }

            EndTutorial();
        }

        private void MoveCharacters(WorkPoint point)
        {
            for (int i = 0; i < 2; i++)
            {
                _movePointController.OnSelectedPoint(point);
            }
        }
        
        private void SetCharacters()
        {
            if (_isCharacterCreated) return;
            _characterInitializer.SetCreatHumanoid(_humanoid);
            _enemyCharacterInitializer.StartSpawning();
            _isCharacterCreated = true;
        }
        private IEnumerator WaitForKeyPress()
        {
            while (!_keyPressed)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    HandleEscapeKey();
                }
                else if (Input.anyKeyDown)
                {
                    _keyPressed = true;
                    StopCoroutine(ChangeImageBrightness());
                    ChangeImageBrightness(1f);
                    
                    if (_isChangedColor)
                    {
                        _currentImageIndex++;
                    }
                }

                yield return null;
            }
        }
        
        private void EndTutorial()
        {
            StopCoroutine(RunTutorial());
            OnEndTutorial?.Invoke();
        }

        private IEnumerator ChangeImageBrightness()
        {
            _currentColor=_currentImage.color;
            
            bool increasingBrightness = true; 
            float currentBrightness = 1.0f; 
            float brightnessChangeStep = 0.03f; 
            float brightnessChangeInterval = 0.01f; 
            
            while (!_keyPressed) 
            {
                if (increasingBrightness && currentBrightness >= 1.0f)
                {
                    increasingBrightness = false;
                }
                else if (!increasingBrightness && currentBrightness <= 0)
                {
                    increasingBrightness = true;
                }

                ChangeImageBrightness(currentBrightness);

                if (increasingBrightness)
                {
                    currentBrightness += brightnessChangeStep;
                }
                else
                {
                    currentBrightness -= brightnessChangeStep;
                }

                yield return new WaitForSeconds(brightnessChangeInterval);
            }
            ChangeImageBrightness(1f);
        }

        private void ChangeImageBrightness(float brightness)
        {
            _currentImageIndex=Math.Clamp(_currentImageIndex,0,_images.Count-1);
            _currentImage = _images[_currentImageIndex];
            _currentImage.color = new Color(brightness, brightness, brightness);
        }
        
        private void ChangeImageAndBrightness()
        {
            StartCoroutine(ChangeImageBrightness());
            
            _currentImageIndex=Math.Clamp(_currentImageIndex,0,_images.Count);
        }

        public void SetImages(List<Image> images)
        {
            _images=images;
        }
    }
}

public enum TutorialDialogKey
{
    WelcomeToTheTutorial,
    WarningTutorial,
    StoreTutorial,
    PointUpTutorial,
    HidePanelTutorial,
    TimePanelTutorial,
    MoneyPanelTutorial,
    MenuPanelTutorial,
    MoveBetweenTutorial,
    RadiusAttackTutorial,
    TakeMedicalKit,
    ReadyTutorial,
}

//  Здравия желаю Капитан! Зомби наступают по всем фронтам и  мы сильно ограничены по времени, поэтому постараюсь быстро ввести тебя в курс дела.
//  Будь внимателен, я буду подсвечивать элементы управления и обьяснять их назначения. Не перебивай, все вопросы потом.
//  Это меню магазина, там ты можешь купить бойцов и оборудование для проведения операций
//  Повышает уровень позиции, попутно увеличиваются навыки находящегося на них бойца
//  скрывает нижнюю панель управления
//  Ты можешь контролировать скорость игры и управлять бойцами 
//  твой кредитный баланс
//  выход в меню
//  Ты можешь перемещать бойцов между позициями в зависимости от поставлвленых задач
//  У каждой боевой единицы визуально отображается радиус атаки
//  Так же могут быть дополнительные виды оружия
//  после того как твой отряд будет готов можешь начать операцию
//Если во время сражения кто то получил повреждения,  то при наличии аптечки можно восстановить здоровье
//  По завершению операции будет предоставлен отчет
//
//
//
//
//
// 