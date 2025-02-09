using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;

namespace Services
{
    public class UpgradeBranch:MonoCache
    {
        [SerializeField] private  UpgradeGroup _branchGroup; 
        private  List<Upgrade> _upgrades  = new List<Upgrade>(); 
        public UpgradeGroup GetUpgradeBranch => _branchGroup;
        private  List<BranchPoint> _points  = new List<BranchPoint>(); 
        
        public void AddUpgrade(Upgrade upgrade)
        {
            if (upgrade != null)
            {
                _upgrades.Add(upgrade);
            }
        }

        public void UpdateUI()
        {
            for (int i = 0; i < _points.Count; i++)
            {
                _points[i].Initialize(_upgrades[i]);
            }
        }

        private void CreateUIElementForUpgrade(Upgrade upgrade)
        {
          
        }
    }
}

public class BranchPoint:MonoCache
{
    [SerializeField] private  UpgradeType _upgradetype; 
    [SerializeField] private  int _id; 
     private  bool _lock; 
     private  Sprite _iconUpgrade; 
     private  Sprite _iconLock; 
     private TextMeshProUGUI _info; 
     private TextMeshProUGUI _price;
     private string _name;

     public UpgradeType GetUpgradeType => _upgradetype;
     public int GetId => _id;
     public bool IsLock => _lock;
     public Sprite GetIconUpgrade => _iconUpgrade;
     public Sprite GetIconLock => _iconLock;
     public TextMeshProUGUI GetInfo => _info;
     public TextMeshProUGUI GetPrice => _price;
     public string GetName => _name;
     
     
     public void Initialize(Upgrade data)
     {
         _upgradetype= data.Type;
         _id= data.Id;
         _lock= data.Lock;
         _iconUpgrade= data.IconUpgrade;
         _iconLock= data.IconLock;
         _info= data.Info;
         _price= data.Price;
         _name= data.Name;
     }
}