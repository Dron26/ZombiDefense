using System.Collections;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

public class CharacterStoreRotation : MonoCache
{
    public float rotationSpeed = 20f;
    private Quaternion _startRotation;
    private Coroutine _rotationCoroutine;
    private bool _isRotating;

    private void Awake()
    {
        _startRotation = transform.rotation;
        _isRotating = false;
    }

    protected override void OnEnabled()
    {
        Rotate();
    }


    protected override void OnDisabled()
    {
        base.OnDisable();
    }

    void OnDisable()
    {
        StopRotation();
    }

    private IEnumerator RotateObject()
    {
        while (_isRotating)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            float newYRotation = currentRotation.y - rotationSpeed * Time.deltaTime;

            if (newYRotation < 0)
            {
                newYRotation += 360f;
            }

            transform.rotation = Quaternion.Euler(currentRotation.x, newYRotation, currentRotation.z);

            yield return null;
        }
    }

    private void Rotate()
    {
        transform.rotation = _startRotation;

        if (!_isRotating)
        {
            _isRotating = true;
            _rotationCoroutine = StartCoroutine(RotateObject());
        }
    }

    public void StopRotation()
    {
        if (_isRotating)
        {
            _isRotating = false;
            if (_rotationCoroutine != null)
            {
                StopCoroutine(_rotationCoroutine);
            }
        }
    }
}