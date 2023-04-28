using Infrastructure.AIBattle;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Humanoids.AbstractLevel
{
    [RequireComponent(typeof(PlayerCharactersStateMachine))]
    public abstract class Humanoid : MonoCache
    {
        private Vector3 _firstPosition;

        public abstract float GetRangeAttack();
        public abstract bool IsLife();
        public abstract int GetLevel();
        public abstract int GetPrice();
        public abstract int GetDamage();

        public Vector3 ReadFirstPosition() =>
            _firstPosition;

        public void RecordFirstPosition(Vector3 firstPosition) =>
            _firstPosition = firstPosition;

        public void InitPosition(Vector3 newPosition) =>
            transform.position = newPosition;

        public abstract void ApplyDamage(int getDamage);

        public abstract int GetDamageDone();

        public abstract int DamageReceived();

        public abstract int TotalPoints();
    }
}