using HumanoidsUI.AbstractLevel.SimpleWarriors;
using Infrastructure.AIBattle;
using UnityEngine;

namespace HumanoidsUI.Cyber
{
    public class CyberArcher : CyberMen
    {
        private const int Level = 6;
        private const int Price = 256;
        
        public override int GetLevel() => 
            Level;

        public override int GetPrice() =>
            Price;

    }
}