using System.Collections.Generic;
using DG.Tweening;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SceneBattle.Store
{
    class Panel : MonoCache
    {
        private Image _characterImagePrefab;
        private float _circleRadius = 100f;
        private List<Sprite> _characterSprites;
        private List<Humanoid> _availableCharacters;
        private Dictionary<Button, Humanoid> _buttonCharacterMap;
        private GameObject _confirmationDialog;
        public float characterDisplayTime = 1f;
        private List<Image> characterImages;
        private int currentIndex = 0;


        private PlayerCharacterInitializer _characterInitializer;

        public void ShowAvaibleCharacters()
        {
            throw new System.NotImplementedException();
        }

        private void Awake()
        {
            characterImages = new List<Image>();
            _buttonCharacterMap = new Dictionary<Button, Humanoid>();
        }

        public void Initialize(List<Humanoid> availableCharacters, PlayerCharacterInitializer initializer)
        {
            _characterInitializer = initializer;
            _availableCharacters = availableCharacters;
            characterImages.Clear();
            _buttonCharacterMap.Clear();
            currentIndex = 0;

            // Find sprites for each character type
            _characterSprites = new List<Sprite>();
            foreach (Humanoid humanoid in availableCharacters)
            {
                if (!_characterSprites.Contains(humanoid.sprite))
                {
                    _characterSprites.Add(humanoid.sprite);
                }
            }
            
            

            // Create character image game objects and add them to the list
            foreach (Sprite sprite in _characterSprites)
            {
                Image image = Instantiate(_characterImagePrefab, transform);
                image.sprite = sprite;
                characterImages.Add(image);
            }

            // Calculate positions for each character image
            float angleIncrement = 360f / _characterSprites.Count;
            float angle = 0f;
            for (int i = 0; i < _characterSprites.Count; i++)
            {
                float x = Mathf.Sin(angle * Mathf.Deg2Rad) * _circleRadius;
                float y = Mathf.Cos(angle * Mathf.Deg2Rad) * _circleRadius;
                Vector2 position = new Vector2(x, y);
                characterImages[i].rectTransform.anchoredPosition = position;
                angle += angleIncrement;
            }

            // Add button click listeners
            foreach (Humanoid humanoid in availableCharacters)
            {
                foreach (Image image in characterImages)
                {
                    if (image.sprite == humanoid.sprite)
                    {
                        Button button = image.GetComponent<Button>();
                        button.onClick.AddListener(() => ShowConfirmationDialog(humanoid));
                        _buttonCharacterMap.Add(button, humanoid);
                        break;
                    }
                }
            }
        }

        public void ShowConfirmationDialog(Humanoid humanoid)
        {
            _confirmationDialog.SetActive(true);
            Text message = _confirmationDialog.GetComponentInChildren<Text>();
            message.text = "Do you want to buy " + humanoid.name + "?";
            Button confirmButton = _confirmationDialog.transform.Find("ConfirmButton").GetComponent<Button>();
            Button cancelButton = _confirmationDialog.transform.Find("CancelButton").GetComponent<Button>();
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(() => BuyCharacter(humanoid));
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(CloseConfirmationDialog);
        }

        public void CloseConfirmationDialog()
        {
            _confirmationDialog.SetActive(false);
        }

        public void BuyCharacter(Humanoid humanoid)
        {
            print("TODO: Implement buying the character");
        }

        public void ShowCharacters()
        {
            currentIndex = 0;
            ShowCharacter();
        }

        private void ShowCharacter()
        {
            if (currentIndex >= characterImages.Count)
            {
                return;
            }

            Image currentCharacterImage = characterImages[currentIndex];
            currentCharacterImage.gameObject.SetActive(true);
            currentCharacterImage.transform.localScale = Vector3.zero;

            currentCharacterImage.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            currentIndex++;

            DOVirtual.DelayedCall(characterDisplayTime, () =>
            {
                ShowCharacter();
            });
        }

        private void Start()
        {
            ShowCharacters();
        }
    }
}