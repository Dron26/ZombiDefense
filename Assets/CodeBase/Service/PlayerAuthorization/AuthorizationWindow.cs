using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.UI;

namespace Service.PlayerAuthorization
{
    public class AuthorizationWindow:MonoCache
    {
        [SerializeField] private Button _applyButton;
        [SerializeField] private Button _denyButton;

        private IAuthorization _authorization;
        
        protected override void  OnEnabled()
        {
            _applyButton.onClick.AddListener(Authorize);
            _denyButton.onClick.AddListener(Hide);

            if (_authorization == null)
                _authorization = AllServices.Container.Single<IAuthorization>();

        }

        protected override void  OnDisabled()
        {
            _applyButton.onClick.RemoveListener(Authorize);
            _denyButton.onClick.RemoveListener(Hide);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
            
            Time.timeScale = ConstantsData.TimeScaleResume;
        }
        
        private void Authorize()
        {
            Debug.Log("Authorize");
            _authorization.OnAuthorizeSuccessCallback += RequestPersonalProfileDataPermission;
            _authorization.OnAuthorizeErrorCallback += ShowAuthorizeError;
            _authorization.Authorize();
            Hide();
        }

        private void RequestPersonalProfileDataPermission()
        {
            Debug.Log("RequestPersonalProfileDataPermission");
            _authorization.OnAuthorizeSuccessCallback -= RequestPersonalProfileDataPermission;
            _authorization.OnRequestErrorCallback += ShowRequestError;
            _authorization.RequestPersonalProfileDataPermission();
        }

        private void ShowAuthorizeError(string error)
        {
            Debug.Log($"ServiceAuthorization ShowAuthorizeError {error}");
            _authorization.OnAuthorizeErrorCallback -= ShowAuthorizeError;
        }

        private void ShowRequestError(string error)
        {
            Debug.Log($"ServiceAuthorization ShowRequestError {error}");
            _authorization.OnRequestErrorCallback -= ShowRequestError;
        }

    }
}