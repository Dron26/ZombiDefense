using EnemiesUI.AbstractEntity;
using Infrastructure.AIBattle;
using UnityEngine;

namespace EnemiesUI.Aliens
{
    public class ErisAlien : Alien
    {
        private int Level=1;
        public override int GetLevel() => 
            Level;
    }
}