using System;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Data;
using Enemies.AbstractEntity;
using Infrastructure.Location;
using Services;

namespace Interface
{
    public interface IGameEventBroadcaster:IService
    {
        event Action OnSetActiveHumanoid;
        event Action<Character> OnCharacterDie;
        event Action OnHumanoidDie;
        event Action<Character> OnSetActiveCharacter;
        event Action LastHumanoidDie;
        event Action OnSetInactiveEnemy;
        event Action<WorkPoint> OnSelectedNewPoint;
        event Action<Character> OnSelectedHumanoid;
        event Action<int> OnChangeEnemiesCountOnWave;
        event Action<Enemy> OnEnemyDeath;
        event Action OnLastEnemyRemained;
        event Action OnLocationCompleted;
        event Action OnClearSpawnData;
        event Action OnCharacterLevelUp;
        event Action OnMoneyEnough;
        event Action OnActivatedSpecialTechnique;
        event Action<int> OnOnSetMaxEnemy;
        event Action<CharacterData> OnBoughtCharacter;
        event Action<BoxData> OnBoughtBox;
        event Action<int> OnMoneyChanged;
        public void InvokeOnSetActiveHumanoid();
        public void InvokeOnCharacterDie(Character character);
        public void InvokeOnOnHumanoidDieDie();
        public void InvokeLastHumanoidDie();
        
        public void InvokeOnSelectedNewPoint(WorkPoint point);
        public void InvokeOnSelectedHumanoid(Humanoid humanoid);
        public void InvokeOnChangeEnemiesCountOnWave(int count);
        public void InvokeOnEnemyDeath(Enemy enemy);
        public void InvokeLastEnemyRemained();
        public void InvokeOnLocationCompleted();
        public void InvokeOnUpgradePurchased(int upgradeId);
        public void InvokeOnUpgradeRefundedEvent (int upgradeId);
        public void InvokeOnSetActiveCharacter(Character character);
        
        public void InvokeOnCharacterLevelUp();
        public void InvokeOnMoneyEnough();
        public void InvokeOnActivatedSpecialTechnique();
        
        public void InvokeOnSetMaxEnemy(int count);
        
        public void InvokeOnBoughtCharacter(CharacterData data);
        public void InvokeOnBoughtBox(BoxData data);
        public void InvokeOnMoneyChanged(int Money);
    }
}