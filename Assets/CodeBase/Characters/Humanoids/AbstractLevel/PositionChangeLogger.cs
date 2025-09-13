using UnityEngine;

public class PositionChangeLogger : MonoBehaviour
{
    private Vector3 _lastPosition;
    
    private void Start()
    {
        _lastPosition = transform.position;
    }
    
    private void Update()
    {
        if (transform.position != _lastPosition)
        {
            Debug.Log($"Position changed from {_lastPosition} to {transform.position}", this);
            _lastPosition = transform.position;
            Debug.Log($"Global: {transform.position}, Local: {transform.localPosition}");
            // Можно добавить стек вызовов, чтобы понять, кто вызвал изменение
            Debug.Log("Call stack:\n" + GetStackTrace());
        }
    }
    
    private string GetStackTrace()
    {
        // Получаем стек вызовов (может быть не всегда точным из-за оптимизаций)
        return new System.Diagnostics.StackTrace(true).ToString();
    }
}