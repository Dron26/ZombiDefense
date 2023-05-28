using System;
using System.Collections;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Infrastructure.Logic
{
    [DisallowMultipleComponent]
    public class LoadingCurtainOld : MonoCache
    {
        public KeyCode _keyCode = KeyCode.Space;
        public GameObject loadingInfo, loadingIcon;
        private AsyncOperation async;
        public CanvasGroup _canvasGroup;
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.interactable=false;
        }

        void Update ()
        {
            if(Input.GetKeyDown(_keyCode)) async.allowSceneActivation = true;
        }
        
        public void Hide(bool isMain) => StartCoroutine(Delay(isMain));
    
    
        private IEnumerator Delay(bool isMain)
        {
            async = SceneManager.LoadSceneAsync(LoadLevel.levelName);
            loadingIcon.SetActive(true);
            loadingInfo.SetActive(false);
            yield return true;
            async.allowSceneActivation = false;
            loadingIcon.SetActive(false);
            loadingInfo.SetActive(true);
        }
    }
}