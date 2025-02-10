using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BranchPoint : MonoCache
{
    [SerializeField] private UpgradeType _upgradeType;
    [SerializeField] private int _id;
    [SerializeField] private bool _lock;
    [SerializeField] private Image _iconRenderer;
    [SerializeField] private Image _iconLock;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _price;

    private Sprite _iconUpgrade;

    public UpgradeType GetUpgradeType => _upgradeType;
    public int GetId => _id;
     private string _description;

    public void Initialize(Upgrade data)
    {
        _upgradeType = data.Type;
        _id = data.Id;
        _lock = data.Lock;
        _iconUpgrade = data.IconUpgrade;
        _iconLock.sprite = data.IconLock;
        _name.text  = data.Name;

        _description = data.Description;
        _price.text = data.Price;
        RefreshUI();
    }
    
    public void RefreshUI()
    {
        _iconRenderer.sprite =  _iconUpgrade;
        _iconLock.sprite = _iconLock.sprite;
        _iconLock.gameObject.SetActive(_lock);
        _name.gameObject.SetActive(!_lock);
        _price.gameObject.SetActive(!_lock);
    }
}