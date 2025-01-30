using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Language
{
    public class ButtonSwitchLanguage : MonoCache
    {
        [SerializeField]private Image _iconCheck;
        private string _name;
        public string Name => _name;
        public void SetIconCheck(bool isActive) => 
            _iconCheck.gameObject.SetActive(isActive);
        
        public void SetName(string name) =>
            _name = name;
    }
}