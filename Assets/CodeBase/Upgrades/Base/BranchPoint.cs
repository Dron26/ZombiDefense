using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BranchPoint : MonoCache
{
    [SerializeField] private UpgradeType _upgradeType;
    [SerializeField] private int _id;
    [SerializeField] private bool _lock;
    [SerializeField] private Image _iconLock;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _price;
    private Upgrade _upgrade;
    public Button Button;
    public Upgrade Upgrade => _upgrade;
    public UpgradeType GetUpgradeType => _upgradeType;
    public List<float> UpgradesValue => _upgrade.UpgradesValue;
    public int GetId => _id;
    private string _description;

    public void Initialize(Upgrade upgrade)
    {
        _upgrade = upgrade;
        _upgradeType = _upgrade.Type;
        _id = _upgrade.Id;
        _lock = _upgrade.Lock;
        _name.text  = _upgrade.Name;
        _icon.sprite = _upgrade.Icon;
        _description = _upgrade.Description;
        _price.text ="$ "+ _upgrade.Cost;
        Button.interactable = !_lock;
    }
    
    public void IsLock(bool isLock)
    {
        _iconLock.gameObject.SetActive(isLock);
        Button.interactable=!isLock;
    }
    
    
}