using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.Inits;
using Infrastructure.Points;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Infrastructure.Tutorial
{
    public class TutorialLevel : MonoCache
    {
        [SerializeField] private TMP_Text tutorialText;
        private Dictionary<Image, Image> _showObjects;
        [SerializeField] private List<Image> _buttons;
        [SerializeField] private List<Image> _urrows;
        [SerializeField] private Humanoid _humanoid;
        [SerializeField] private WorkPoint _firstPoint;
        [SerializeField] private WorkPoint _secondPoint;
        [SerializeField] private Image _dimm;
        [SerializeField] private GameObject _doorFirst;
        [SerializeField] private GameObject _doorSecond;
        private int _eulerDoor = 129;
        private Quaternion _rotationDoorFirst;
        private Quaternion _rotationDoorSecond;
        private Image _currentImage;
        private LeanLocalizedTextMeshProUGUI _localizedTextMesh;
        private int currentDialogIndex = 0;
        private List<string> _dialogTexts;
        private Color _currentColor;
        private Coroutine tutorialCoroutine;
        private int _numberForStartChangeColor = 2;
        private bool _keyPressed = false;
        private int _currentImageIndex = 0;
        public Action OnEndTutorial;
        private bool _isCharacterCreated = false;
        private PlayerCharacterInitializer _characterInitializer;
        private EnemyCharacterInitializer _enemyCharacterInitializer;
        private int numberForCreatedCharacter = 9;
        private bool _isChangedColor = false;
        private MovePointController _movePointController;
        private SceneInitializer _initializer;

        private void Start()
        {
            _localizedTextMesh = tutorialText.GetComponent<LeanLocalizedTextMeshProUGUI>();
            _characterInitializer = GetComponentInChildren<PlayerCharacterInitializer>();
            _enemyCharacterInitializer = GetComponentInChildren<EnemyCharacterInitializer>();

            FillDictionary();
        }

        private void FillDictionary()
        {
            _showObjects = new Dictionary<Image, Image>();

            for (int i = 0; i < _buttons.Count; i++)
            {
                _urrows[i].gameObject.SetActive(false);
                _showObjects.Add(_buttons[i], _urrows[i]);
            }
        }

        private void HandleEscapeKey()
        {
            EndTutorial();
        }

        public void Initialize(SceneInitializer sceneInitializer)
        {
            _dialogTexts = new List<string>();
            _currentImageIndex = 0;
            _dimm.enabled = true;
            _movePointController = sceneInitializer.GetMovePointController();
            _initializer = sceneInitializer;
            _rotationDoorFirst = _doorFirst.transform.rotation;
            _rotationDoorSecond = _doorSecond.transform.rotation;

            foreach (TutorialDialogKey dialog in Enum.GetValues(typeof(TutorialDialogKey)))
            {
                _dialogTexts.Add(dialog.ToString());
            }

            tutorialCoroutine = StartCoroutine(RunTutorial());
        }

        public IEnumerator RunTutorial()
        {
            yield return new WaitForSeconds(1f);
            _initializer.GetHudPanel().SwitchPanelState(false);
           

            foreach (string dialogText in _dialogTexts)
            {
                _keyPressed = false;
                _localizedTextMesh.TranslationName = dialogText;

                if (currentDialogIndex >= _numberForStartChangeColor && _currentImageIndex < _showObjects.Count)
                {
                    _isChangedColor = true;
                    ChangeImageAndBrightness();
                }
                else
                {
                    _isChangedColor = false;
                }

                if (currentDialogIndex == numberForCreatedCharacter)
                {
                    ChangeImageBrightness(1f);
                    StopCoroutine(ChangeImageBrightness());
                    tutorialText.color = Color.white;
                    
                }

                if (currentDialogIndex == numberForCreatedCharacter + 1)
                {
                    MoveCharacters(_firstPoint);
                }

                if (currentDialogIndex == numberForCreatedCharacter + 2)
                {
                    MoveCharacters(_secondPoint);
                }

                if (currentDialogIndex == numberForCreatedCharacter + 3)
                {
                    SetCharacters();
                    OpenDoor();
                }

                yield return WaitForKeyPress();

                currentDialogIndex++;
            }

            EndTutorial();
        }

        private void OpenDoor()
        {
            _initializer.GetSaveLoad().GetActiveEnemy()[0].GetComponent<NavMeshAgent>().speed = 1;
            _rotationDoorFirst.eulerAngles=new Vector3(0,_eulerDoor,0);
            _rotationDoorSecond.eulerAngles=new Vector3(0,-_eulerDoor,0);
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
            _dimm.enabled = false;
            if (_isCharacterCreated) return;
            _characterInitializer.SetCreatedCharacter(_humanoid);
            _enemyCharacterInitializer.SetWaveData();
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
                    
                    if (currentDialogIndex>=_numberForStartChangeColor&&_currentImageIndex<_showObjects.Count)
                    {
                        _urrows[_currentImageIndex].gameObject.SetActive(false);
                        StopCoroutine(ChangeImageBrightness());
                        ChangeImageBrightness(1f);
                    }
                    
                    
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
            _initializer.GetHudPanel().SwitchPanelState(true);
            StopCoroutine(RunTutorial());
            OnEndTutorial?.Invoke();
        }

        private IEnumerator ChangeImageBrightness()
        {
            
            bool increasingBrightness = true; 
            float currentBrightness = 1.0f; 
            float brightnessChangeStep = 0.03f; 
            float brightnessChangeInterval = 0.01f; 
            
            _currentColor=_currentImage.color;
            _urrows[_currentImageIndex].gameObject.SetActive(true);
            
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
            
            _currentImage.color = new Color(brightness, brightness, brightness);
        }
        
        private void ChangeImageAndBrightness()
        {
            _currentImage=_buttons[_currentImageIndex];
            StartCoroutine(ChangeImageBrightness());
        }

        public void SetImages(List<Image> images)
        {
            _buttons=images;
        }
    }
}

public enum TutorialDialogKey
{
    WelcomeToTheTutorial,
    WarningTutorial,
    StoreTutorial,
    PointUpTutorial,
    AdditionalTutorial,
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