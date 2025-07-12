using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using YG.LanguageLegacy;

public class BranchPoint : MonoCache
{
    [SerializeField] private UpgradeType _upgradeType;
    [SerializeField] private int _id;
    [SerializeField] private bool _lock;
     private TMP_Text _name;
     private TMP_Text _price;
     private Text _descriptionText;
    public Button Button;
    private Image _lockIcon;
    private LanguageYG _languageYG;
    private Upgrade _upgrade;
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

        NameText text = GetComponentInChildren<NameText>();
        _name = text.gameObject.GetComponent<TMP_Text>();
        _name.text=_upgrade.Name;
         _price=GetComponentInChildren<PriceText>().GetComponent<TMP_Text>();
        Button=GetComponent<Button>();
        GetComponentInChildren<IconUpgrade>().GetComponent<Image>().sprite = _upgrade.Icon;
        _description = _upgrade.Description;
        _price.text ="$ "+ _upgrade.Cost;
        Button.interactable = !_lock;
        _descriptionText=GetComponent<Text>();
        _descriptionText.text = _description;
        _lockIcon = GetComponentInChildren<IconLock>().GetComponent<Image>();
        
        _upgrade.SetNewText( _descriptionText.text);
        
        _languageYG = GetComponent<LanguageYG>();
        _languageYG.OnSwitchLanguage+= OnChangeDescription;
        SetLock(_lock);
    }

    private void OnChangeDescription()
    {
        _upgrade.SetNewText(_descriptionText.text);
    }

    public void SetLock(bool isLock)
    {
        _lockIcon.enabled=isLock;
        Button.interactable=!isLock;
    }
    
    private void OnDestroy()
    {
        _languageYG.OnSwitchLanguage+= OnChangeDescription;
    }
    
}

