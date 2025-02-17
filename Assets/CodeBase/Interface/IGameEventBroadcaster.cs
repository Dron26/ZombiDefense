using System;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Enemies.AbstractEntity;
using Infrastructure.Location;
using Services;

namespace Interface
{
    public interface IGameEventBroadcaster:IService
    {
        event Action OnSetActiveHumanoid;
        event Action LastHumanoidDie;
        event Action OnSetInactiveEnemy;
        event Action<WorkPoint> OnSelectedNewPoint;
        event Action<Character> OnSelectedNewCharacter;
        event Action<int> OnChangeEnemiesCountOnWave;
        event Action<Enemy> OnEnemyDeath;
        event Action LastEnemyRemained;
        event Action OnLocationCompleted;
        event Action OnClearSpawnData;

        public void InvokeOnSetActiveHumanoid();
        public void InvokeLastHumanoidDie();
        public void InvokeOnSelectedNewPoint(WorkPoint point);
        public void InvokeOnSelectedNewCharacter(Character character);
        public void InvokeOnChangeEnemiesCountOnWave(int count);
        public void InvokeOnEnemyDeath(Enemy enemy);
        public void InvokeLastEnemyRemained();
        public void InvokeOnLocationCompleted();
        public void InvokeOnUpgradePurchased(int upgradeId);
        public void InvokeOnUpgradeRefundedEvent (int upgradeId);
    }
}