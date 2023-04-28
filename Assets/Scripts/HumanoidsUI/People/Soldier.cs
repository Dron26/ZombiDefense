using HumanoidsUI.AbstractLevel.SimpleWarriors;

namespace HumanoidsUI.People
{
    public class Soldier : PeopleMen
    {
        private const int Level = 1;
        private const int Price = 1;

        public override int GetLevel() =>
            Level;

        public override int GetPrice() =>
            Price;
    }
}