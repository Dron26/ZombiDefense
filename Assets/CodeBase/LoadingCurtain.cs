using System;
using UnityEngine;
using System.Collections;
using UI;

public class LoadingCurtain : MonoBehaviour
{
    [SerializeField]private GameObject loadingInfo;
    [SerializeField]private GameObject loadedInfo;
    [SerializeField]private GameObject loadingIcon;
    [SerializeField] private GameObject _panel;

    private CanvasGroup _canvasGroup;
    public Action OnStartLoading;
    public Action OnClicked;
    private GlobalTimer _globalTimer;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _canvasGroup = _panel.GetComponent<CanvasGroup>();
        
    }

    public void StartLoading()
    {
        Debug.Log("Loading");
        OnStartLoading?.Invoke();
        _panel.SetActive(true);
        _canvasGroup.blocksRaycasts = true;
        loadingIcon.SetActive(true);
        loadingInfo.SetActive(true);
        loadedInfo.SetActive(false);
    }
    
    private IEnumerator LoadSceneAsync()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.5f);
        loadingInfo.SetActive(false);
        loadedInfo.SetActive(true);
        loadingIcon.SetActive(false);
        

        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        
        _panel.SetActive(false);
        _canvasGroup.blocksRaycasts = false;
        OnClicked?.Invoke();
    }

    public void OnLoaded( )
    {
        StartCoroutine(LoadSceneAsync());
    }
}