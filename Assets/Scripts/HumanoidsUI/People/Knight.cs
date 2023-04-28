using HumanoidsUI.AbstractLevel.SimpleWarriors;
using Infrastructure.AIBattle;
using UnityEngine;

namespace HumanoidsUI.People
{
    public class Knight : PeopleMen
    {
        private const int Level = 3;
        private const int Price = 16;
        
        public override int GetLevel() => 
            Level;

        public override int GetPrice() =>
            Price;
    }
}