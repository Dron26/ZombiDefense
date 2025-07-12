using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuntimeLogger : MonoBehaviour
{
    public static RuntimeLogger Instance;

    [SerializeField] private Text logText;
    private readonly Queue<string> _logs = new();
    private const int MaxLines = 20;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LogError(string message)
    {
        AddLog("[Error] " + message);
    }

    public void Log(string message)
    {
        AddLog(message);
    }

    private void AddLog(string message)
    {
        if (_logs.Count >= MaxLines)
            _logs.Dequeue();

        _logs.Enqueue(message);
        UpdateLogText();
    }

    private void UpdateLogText()
    {
        logText.text = string.Join("\n", _logs);
    }
}