using System.Collections.Generic;
using EnemiesUI.AbstractEntity;
using HumanoidsUI.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UI.SceneSetArmy;
using UI.SceneSetArmy.Slots;
using UnityEngine;

namespace UI.Panel
{
    [DisallowMultipleComponent]
    public class UIArmyPanelSettings : MonoCache
    {
        private PlayerSlotsInitializer _playerSlotsInitializer;
        private EnemySlotsInitializer _enemySlotsInitializer;
        
        [SerializeField] private GameObject _slotPlayer;
        [SerializeField] private GameObject _slotEnemy;
        
        public void Initialize(List<HumanoidUI> platoon, List<Enemy> platoonEnemy,ArmyInitializer armyInitializer) => 
            InitializeGroupUnits(platoon,platoonEnemy, armyInitializer);

        private void InitializeGroupUnits(List<HumanoidUI> platoon, List<Enemy> platoonEnemy, ArmyInitializer armyInitializer)
        {
            _playerSlotsInitializer=GetComponentInChildren<PlayerSlotsInitializer>();
            _enemySlotsInitializer=GetComponentInChildren<EnemySlotsInitializer>();
            
            _playerSlotsInitializer.Initialize(platoon,_slotPlayer,armyInitializer);
            _enemySlotsInitializer.Initialize(platoonEnemy,_slotEnemy,armyInitializer);
        }
    }
}