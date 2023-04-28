using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.LuckySpin
{
    public class CanvasRouletteEffect : MonoCache
    {
        [SerializeField] private CanvasLuckySpin _canvasLuckySpin;
        
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _result;
        [SerializeField] private TMP_Text _nameGift;
        
        [SerializeField] private Sprite _imageX1;
        [SerializeField] private Sprite _imagePoints550;
        [SerializeField] private Sprite _imageAgainSpin;
        [SerializeField] private Sprite _imageX4;
        [SerializeField] private Sprite _imageCoins1000;
        [SerializeField] private Sprite _imageX8;
        [SerializeField] private Sprite _imageCoins100;
        [SerializeField] private Sprite _imagePoints1000;
        
        [SerializeField] private TMP_Text _nameX1;
        [SerializeField] private TMP_Text _namePoints550;
        [SerializeField] private TMP_Text _nameAgainSpin;
        [SerializeField] private TMP_Text _nameX4;
        [SerializeField] private TMP_Text _nameCoins1000;
        [SerializeField] private TMP_Text _nameX8;
        [SerializeField] private TMP_Text _nameCoins100;
        [SerializeField] private TMP_Text _namePoints1000;

        [SerializeField] private TMP_Text[] _resultGiftUp;
        
        private Dictionary<int, GiftUpCard> _cards;
        
        public void ShowResult(int level)
        {
            Init();
            
            _icon.sprite = _cards[level].IconGift;
            _nameGift.text = _cards[level].NameGift;

            if (_resultGiftUp.Length != 0)
                _result = _resultGiftUp[Random.Range(0, _resultGiftUp.Length)];

            _canvasLuckySpin.gameObject.SetActive(false);
            gameObject.SetActive(true);

            Time.timeScale = 0;
        }

        public void CloseCanvas()
        {
            gameObject.SetActive(false);
            _canvasLuckySpin.gameObject.SetActive(true);

            Time.timeScale = 1;
        }

        private void Init()
        {
            _cards = new Dictionary<int, GiftUpCard>
            {
                [(int)GiftType.X1] = new(_imageX1, _nameX1.text),
                [(int)GiftType.Points550] = new(_imagePoints550, _namePoints550.text),
                [(int)GiftType.AgainSpin] = new(_imageAgainSpin, _nameAgainSpin.text),
                [(int)GiftType.X4] = new(_imageX4, _nameX4.text),
                [(int)GiftType.Coins1000] = new(_imageCoins1000, _nameCoins1000.text),
                [(int)GiftType.X8] = new(_imageX8, _nameX8.text),
                [(int)GiftType.Coins100] = new(_imageCoins100, _nameCoins100.text),
                [(int)GiftType.Points1000] = new(_imagePoints1000, _namePoints1000.text),
            };
        }
    }
}