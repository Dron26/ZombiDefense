using EnemiesUI.AbstractEntity;
using Infrastructure.AIBattle;
using UnityEngine;

namespace EnemiesUI.Aliens
{
    public class MercuryAlien : Alien
    {
        private int Level = 2;

        public override int GetLevel() =>
            Level;
    }
}