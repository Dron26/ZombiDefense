using System;
using Characters.Humanoids.AbstractLevel;
using Enemies.AbstractEntity;
using Infrastructure.Location;

namespace Interface
{
    public class GameEventBroadcaster: IGameEventBroadcaster
    {
        public event Action OnSetActiveHumanoid;
        public event Action LastHumanoidDie;
        public event Action OnSetInactiveEnemy;
        public event Action<WorkPoint> OnSelectedNewPoint;
        public event Action<Character> OnSelectedNewCharacter;
        public event Action<int> OnChangeEnemiesCountOnWave;
        public event Action<Enemy> OnEnemyDeath;
        public event Action LastEnemyRemained;
        public event Action OnLocationCompleted;
        public event Action OnClearSpawnData;

        public void InvokeOnSetActiveHumanoid() => OnSetActiveHumanoid?.Invoke();
        public void InvokeLastHumanoidDie() => LastHumanoidDie?.Invoke();
        public void InvokeOnSelectedNewPoint(WorkPoint point) => OnSelectedNewPoint?.Invoke(point);
        public void InvokeOnSelectedNewCharacter(Character character) => OnSelectedNewCharacter?.Invoke(character);
        public void InvokeOnChangeEnemiesCountOnWave(int count) => OnChangeEnemiesCountOnWave?.Invoke(count);
        public void InvokeOnEnemyDeath(Enemy enemy) => OnEnemyDeath?.Invoke(enemy);
        public void InvokeLastEnemyRemained() => LastEnemyRemained?.Invoke();
        public void InvokeOnLocationCompleted() => OnLocationCompleted?.Invoke();
    }
}