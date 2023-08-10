using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Events;

public class LoadingCurtain : MonoBehaviour
{
    [SerializeField]private GameObject loadingInfo;
    [SerializeField]private GameObject loadedInfo;
    [SerializeField]private GameObject loadingIcon;
    [SerializeField] private GameObject _panel;

    private CanvasGroup _canvasGroup;
    public UnityAction OnClicked;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _canvasGroup = _panel.GetComponent<CanvasGroup>();
        
    }

    public void StartLoading()
    {
        _panel.SetActive(true);
        _canvasGroup.blocksRaycasts = true;
        loadingIcon.SetActive(true);
        loadingInfo.SetActive(true);
        loadedInfo.SetActive(false);
    }
    
    private IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(0.5f);
        loadingInfo.SetActive(false);
        loadedInfo.SetActive(true);
        loadingIcon.SetActive(false);
        

        // Ожидание нажатия кнопки
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