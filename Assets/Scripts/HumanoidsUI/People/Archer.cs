using HumanoidsUI.AbstractLevel.SimpleWarriors;

namespace HumanoidsUI.People
{
    public class Archer : PeopleMen
    {
        private const int Level = 2;
        private const int Price = 4;
        
        public override int GetLevel() => 
            Level;

        public override int GetPrice() =>
            Price;
    }
}