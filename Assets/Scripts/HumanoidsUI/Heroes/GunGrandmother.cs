﻿using HumanoidsUI.AbstractLevel;

namespace HumanoidsUI.Heroes
{
    public class GunGrandmother : Hero
    {
        private const int Level = 11;
        public override int GetLevel() => 
            Level;

        public override int GetPrice() =>
            throw new System.NotImplementedException();
    }
}