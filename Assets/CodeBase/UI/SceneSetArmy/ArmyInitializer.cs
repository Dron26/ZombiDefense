using System.Collections.Generic;
using EnemiesUI.AbstractEntity;
using HumanoidsUI.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
using UI.Panel;
using UI.SceneSetArmy.Slots;
using UnityEngine;
using UnityEngine.Events;

namespace UI.SceneSetArmy
{
    [DisallowMultipleComponent]
    public class ArmyInitializer : MonoCache
    {
        public UnityAction ClickButtonBack;
        
        [SerializeField] private UIArmyPanelSettings _uArmyPanelSettings;
        [SerializeField]private PriceChenger _priceChenger;
        public void Initialize(List<HumanoidUI> platoon, List<Enemy> platoonEnemy, SaveLoadService saveLoadService)
        {
             
            InitializePlatoonsUnit(platoon,platoonEnemy);
            InitializePriceInfo(saveLoadService);
        }
        
        private void InitializePlatoonsUnit( List<HumanoidUI> platoon,List<Enemy> platoonEnemy) => 
            _uArmyPanelSettings.Initialize(platoon,platoonEnemy,this);

        private void InitializePriceInfo(SaveLoadService saveLoadService)
        {
            PlayerSlotsInitializer playerSlotsInitializer= _uArmyPanelSettings.GetComponentInChildren<PlayerSlotsInitializer>();
            _priceChenger.Initialize(playerSlotsInitializer,saveLoadService);
        }

        public void OnClickBack()
        {
            ClickButtonBack?.Invoke();
            _priceChenger.OnClickReset();
        }
    }
}