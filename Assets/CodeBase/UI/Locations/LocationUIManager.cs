using System;
using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UI.Locations;
using UnityEngine;

public class LocationUIManager : MonoCache
{
    [SerializeField] private TMP_Text _cashText;
    [SerializeField] private GameObject _locationContainer;

    private List<LocationUIElement> _locationUIElements;
    private LocationManager _locationManager;
    private Action<int> _onLocationSelected;

    public void Initialize(LocationManager locationManager, Action<int> onLocationSelected)
    {
        _locationManager = locationManager;
        _onLocationSelected = onLocationSelected;
        InitializeUIElements();
    }

    private void InitializeUIElements()
    {
        _locationUIElements = new List<LocationUIElement>();
        foreach (Transform child in _locationContainer.transform)
        {
            var uiElement = child.GetComponent<LocationUIElement>();
            if (uiElement != null)
            {
                _locationUIElements.Add(uiElement);
                uiElement.Initialize(_locationManager.GetLocationById(uiElement.Id));
                uiElement.OnClick += HandleLocationClick;
            }
        }
    }

    private void HandleLocationClick(int id)
    {
        _onLocationSelected?.Invoke(id);
    }

    protected override void OnDisabled()
    {
        foreach (var uiElement in _locationUIElements)
        {
            uiElement.OnClick -= HandleLocationClick;
        }
    }
}