using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoad;
using TMPro;
using UnityEngine;

namespace UI.Buttons
{
    public class CountEnemyPanel : MonoCache
    {
        [SerializeField] private TMP_Text _text;
        private SaveLoadService _saveLoadService;

        public void Initialize(SaveLoadService saveLoadService)
        {
            _saveLoadService= saveLoadService;
            saveLoadService.OnChangeEnemiesCountOnWave += SetCount;
        }

        private void SetCount(int count)
        {
            _text.text=count.ToString();
        }
    }
}