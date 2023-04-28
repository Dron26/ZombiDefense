using HumanoidsUI.AbstractLevel.SimpleWarriors;
using Infrastructure.AIBattle;
using UnityEngine;

namespace HumanoidsUI.Cyber
{
    public class CyberKnight : CyberMen
    {
        private const int Level = 7;
        private const int Price = 512;
       
        public override int GetLevel() => 
            Level;

        public override int GetPrice() =>
            Price;
    }
}