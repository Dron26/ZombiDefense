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

    public UnityAction OnClicked;
    private bool _isLoading = false;
    private static LoadingCurtain instance;
    
    private void Awake()
    {
        // Проверяем, есть ли уже другой экземпляр 
        if (instance != null && instance != this)
        {
            // Уничтожаем текущий экземпляр 
            Destroy(gameObject);
            return;
        }

        // Если нет других экземпляров, делаем текущий экземпляр  неуничтожаемым при загрузке новой сцены
        DontDestroyOnLoad(gameObject);
        instance = this;
        
    }

    public void StartLoading()
    {
        _panel.SetActive(true);
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
        OnClicked?.Invoke();
    }

    public void OnLoaded( )
    {
        StartCoroutine(LoadSceneAsync());
    }
}