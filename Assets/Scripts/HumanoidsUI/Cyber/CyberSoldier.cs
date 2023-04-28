using HumanoidsUI.AbstractLevel.SimpleWarriors;
using Infrastructure.AIBattle;
using UnityEngine;

namespace HumanoidsUI.Cyber
{
    public class CyberSoldier : CyberMen
    {
        private const int Level = 5;
        private const int Price = 128;
        
        public override int GetLevel() => 
            Level;

        public override int GetPrice() =>
            Price;
    }
}