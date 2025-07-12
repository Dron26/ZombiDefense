using System;
using UnityEngine;
using System.Collections;
using UI;
using YG;

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


    public void ShowCurtain()
    {
        Time.timeScale = 1;
        
        Debug.Log("Loading");
        OnStartLoading?.Invoke();
        _panel.SetActive(true);
        _canvasGroup.blocksRaycasts = true;
        loadingIcon.SetActive(true);
    }

    public void OnLoaded()
    {
        StartCoroutine(HideCurtain());
    }
    private IEnumerator HideCurtain()
    {
        _panel.SetActive(true);
        _canvasGroup.blocksRaycasts = true;
        yield return new WaitForSeconds(1f);
        _panel.SetActive(false);
        _canvasGroup.blocksRaycasts = false;
    }
}