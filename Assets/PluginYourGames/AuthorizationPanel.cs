using System.Collections;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.UI;
using YG;
namespace Services.PlayerAuthorization
{
    public class AuthorizationPanel:MonoCache
    {
        [SerializeField] private Button _applyButton;
        [SerializeField] private Button _denyButton;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _panel;

        private void AddListener()
        {
            _button.onClick.AddListener(()=>SwithState(true));
            _applyButton.onClick.AddListener( Authorize);
            _denyButton.onClick.AddListener( ()=>SwithState(false));
        }
        
        private  void RemoveListener()
        {
            _button.onClick.RemoveListener(()=>SwithState(true));
            _applyButton.onClick.RemoveListener(Authorize);
            _denyButton.onClick.RemoveListener(()=>SwithState(false));
        }
        public void Start()
        {
            UpdateButtonActivity();
            AddListener();
        }
        
        private void Authorize()
        {
            Debug.Log("OpenAuthDialog");
            YG2.OpenAuthDialog();
            Debug.Log("OpenAuthDialog2");
            Debug.Log("SwithState");
            SwithState(false);
        }
        
        public void SwithState(bool isActive)
        {
            _panel.gameObject.SetActive(isActive);
        }

        private void UpdateButtonActivity()
        {
            _button.gameObject.SetActive(YG2.player.auth == false);
        }
        
        private void OnDestroy()
        {
            RemoveListener();
        }
    }
}