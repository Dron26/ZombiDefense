using HumanoidsUI.AbstractLevel.SimpleWarriors;
using Infrastructure.AIBattle;
using UnityEngine;

namespace HumanoidsUI.People
{
    public class King : PeopleMen
    {
        private const int Level = 4;
        private const int Price = 64;
        
        public override int GetLevel() => 
            Level;

        public override int GetPrice() =>
            Price;
    }
}