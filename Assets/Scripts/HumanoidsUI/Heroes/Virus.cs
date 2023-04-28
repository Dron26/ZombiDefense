using HumanoidsUI.AbstractLevel;

namespace HumanoidsUI.Heroes
{
    public class Virus : Hero
    {
        private const int Level = 12;

        public override int GetLevel() =>
            Level;

        public override int GetPrice() =>
            throw new System.NotImplementedException();
    }
}