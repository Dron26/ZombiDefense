using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
using UnityEngine;
using UnityEngine.UI;

namespace Service.Reset
{
    public class ResetProgress : MonoCache
    {
        [SerializeField] private SaveLoadService.SaveLoadService _saveLoadService;
        private Button _buttonReset;

        private void Awake()
        {
            _buttonReset=GetComponent<Button>();
            _buttonReset.onClick.AddListener(Reset);
        }
        
        public void Reset()
        {
            _saveLoadService.ResetProgress();
        }
    }
}