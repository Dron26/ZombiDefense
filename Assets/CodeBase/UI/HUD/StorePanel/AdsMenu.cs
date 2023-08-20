using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.StorePanel
{
    public class AdsMenu:MonoCache
    {
        [SerializeField] private Button _buttonShowPanel;
        [SerializeField] private GameObject _adsPanel;

        private void Awake()
        {
            _buttonShowPanel.onClick.AddListener(ShowAdsPanel);
            
            _adsPanel.SetActive(false);
        }

        public void ShowAdsPanel()
        {
            _adsPanel.SetActive(!_adsPanel.activeSelf);
        }
    }
}