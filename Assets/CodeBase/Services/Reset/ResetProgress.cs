using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace Services.Reset
{
    public class ResetProgress : MonoCache
    {
        private Button _buttonReset;

        private void Awake()
        {
            _buttonReset=GetComponent<Button>();
            _buttonReset.onClick.AddListener(Reset);
        }
        
        public void Reset()
        {
            AllServices.Container.Single<DataPersistence>().Reset();
        }
    }
}