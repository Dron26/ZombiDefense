using System;
using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace UI.WarningWindow
{
    [DisallowMultipleComponent]
    public class WindowSwither:MonoCache
    {
        [SerializeField] private  GameObject _windowEmptySlot;
        [SerializeField]private  GameObject _windowEmptyTeam;

        private Dictionary<int, Action> _warningWindows = new ();

        private void Awake() => FillAction();

        private void FillAction()
        {
            _warningWindows.Add(0, ShowEmptySlot);
            _warningWindows.Add(1, ShowEmptyTeam);
        }

        public void ShowWindow(int id) => _warningWindows[id]();
        
        private void ShowEmptySlot() => _windowEmptySlot.gameObject.SetActive(true);

        public void ShowEmptyTeam() => _windowEmptyTeam.gameObject.SetActive(true);
    }
}