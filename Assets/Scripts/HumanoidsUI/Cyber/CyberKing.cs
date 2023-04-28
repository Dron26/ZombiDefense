using HumanoidsUI.AbstractLevel.SimpleWarriors;
using Infrastructure.AIBattle;
using UnityEngine;

namespace HumanoidsUI.Cyber
{
    public class CyberKing : CyberMen
    {
        private const int Level = 8;
        private const int Price = 1024;
        
        public override int GetLevel() => 
            Level;

        public override int GetPrice() =>
            Price;
    }
}