using Infrastructure.Location;
using Service.SaveLoad;
using UnityEditor;

namespace Infrastructure.AIBattle.EnemyAI
{
    public interface IEnemySwitcherState
    {
        public void EnterBehavior();
        public void ExitBehavior();
        public void Init(EnemyStateMachine enemyStateMachine, SaveLoadService saveLoadService);
        
        public void Disable();
        
        public void OnTakeGranadeDamage();
    }
}