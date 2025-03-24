using System;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Data;
using Enemies.AbstractEntity;
using Infrastructure.Location;
using UnityEditor.MPE;

namespace Interface
{
    public class GameEventBroadcaster: IGameEventBroadcaster
    {
        public event Action OnSetActiveHumanoid;
        public event Action<Character>  OnCharacterDie;
        public event Action OnHumanoidDie;
        public event Action<Character> OnSetActiveCharacter;
        public event Action LastHumanoidDie;
        public event Action OnSetInactiveEnemy;
        public event Action<WorkPoint> OnSelectedNewPoint;
        public event Action<Character> OnSelectedHumanoid;
        public event Action<int> OnChangeEnemiesCountOnWave;
        public event Action<Enemy> OnEnemyDeath;
        public event Action OnLastEnemyRemained;
        public event Action OnLocationCompleted;
        public event Action OnClearSpawnData;
        public event Action OnCharacterLevelUp;
        public event Action OnMoneyEnough;
        public event Action OnActivatedSpecialTechnique;
        public event Action<int> OnOnSetMaxEnemy;
        public event Action<int> OnUpgradePurchased;
        public event Action<int> OnUpgradeRefunded;
        public event Action<CharacterData> OnBoughtCharacter;
        public event Action<BoxData> OnBoughtBox;
        public event Action<int> OnMoneyChanged;
        private readonly Dictionary<Type, List<Delegate>> _eventHandlers = new();
        private readonly Dictionary<UpgradeGroupType, Action<Upgrade>> _upgradeEvents;
        
        public GameEventBroadcaster()
        {
            _upgradeEvents = new Dictionary<UpgradeGroupType, Action<Upgrade>>()
            {
                { UpgradeGroupType.Supplies, null },
                { UpgradeGroupType.Weapons, null },
                { UpgradeGroupType.Turrets, null },
                { UpgradeGroupType.Health, null },
                { UpgradeGroupType.Defence, null },
                { UpgradeGroupType.AirStrikes, null },
                { UpgradeGroupType.Squad, null },
                { UpgradeGroupType.SpecialTechnique, null },
                { UpgradeGroupType.Box, null },
                { UpgradeGroupType.Profit, null },
                { UpgradeGroupType.CashLimit, null },
                { UpgradeGroupType.PriceUpdate, null }
            };
        }
        public void Subscribe(UpgradeGroupType groupType, Action<Upgrade> callback)
        {
            if (_upgradeEvents.ContainsKey(groupType))
            {
                _upgradeEvents[groupType] += callback;
            }
        }
        
        public void Unsubscribe(UpgradeGroupType groupType, Action<Upgrade> callback)
        {
            if (_upgradeEvents.ContainsKey(groupType))
            {
                _upgradeEvents[groupType] -= callback;
            }
        }
        
        public void InvokeUpgradeEvent(UpgradeGroupType groupType, Upgrade upgradeId)
        {
            if (_upgradeEvents.ContainsKey(groupType))
            {
                _upgradeEvents[groupType]?.Invoke(upgradeId);
            }
        }
    
        public void InvokeOnSetActiveHumanoid() => OnSetActiveHumanoid?.Invoke();
        public void InvokeOnCharacterDie(Character character) => OnCharacterDie?.Invoke(character);
        public void InvokeOnOnHumanoidDieDie()=>OnHumanoidDie?.Invoke();
        public void InvokeLastHumanoidDie() => LastHumanoidDie?.Invoke();
        public void InvokeOnSelectedNewPoint(WorkPoint point) => OnSelectedNewPoint?.Invoke(point);
        public void InvokeOnSelectedHumanoid(Humanoid humanoid) => OnSelectedHumanoid?.Invoke(humanoid);
        public void InvokeOnChangeEnemiesCountOnWave(int count) => OnChangeEnemiesCountOnWave?.Invoke(count);
        public void InvokeOnEnemyDeath(Enemy enemy) => OnEnemyDeath?.Invoke(enemy);
        public void InvokeLastEnemyRemained() => OnLastEnemyRemained?.Invoke();
        public void InvokeOnLocationCompleted() => OnLocationCompleted?.Invoke();
        public void InvokeOnUpgradePurchased(int upgradeId) => OnUpgradePurchased?.Invoke(upgradeId);

        public void InvokeOnUpgradeRefundedEvent(int upgradeId) => OnUpgradeRefunded?.Invoke(upgradeId);
        public void InvokeOnSetActiveCharacter(Character character)=>OnSetActiveCharacter?.Invoke(character);
        public void InvokeOnCharacterLevelUp()=>OnCharacterLevelUp?.Invoke();
        public void InvokeOnMoneyEnough()=>OnMoneyEnough?.Invoke();
        public void InvokeOnActivatedSpecialTechnique() => OnActivatedSpecialTechnique?.Invoke();
        public void InvokeOnSetMaxEnemy(int count)=>OnOnSetMaxEnemy?.Invoke(count);
        
        public void InvokeOnBoughtCharacter(CharacterData data)=>OnBoughtCharacter?.Invoke(data);
        public void InvokeOnBoughtBox(BoxData data)=>OnBoughtBox?.Invoke(data);
        public void InvokeOnMoneyChanged(int money) => OnMoneyChanged(money);
    }
}